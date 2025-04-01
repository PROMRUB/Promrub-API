using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Promrub.Services.API.Models.Authentications;
using Promrub.Services.API.Utils;
using Serilog;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Promrub.Services.API.Authentications
{
    public abstract class AuthenticationHandlerProxyBase : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private static readonly MemoryCache FailedAttemptsCache = new MemoryCache(new MemoryCacheOptions());
        protected abstract Task<User>? AuthenticateBasic(string orgId, byte[]? jwtBytes, HttpRequest request);
        protected abstract Task<User>? AuthenticateBearer(string orgId, byte[]? jwtBytes, HttpRequest request);

        protected AuthenticationHandlerProxyBase(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ipAddress = Context.Connection.RemoteIpAddress?.ToString();

            if (!Request.Headers.TryGetValue("Authorization", out var authData))
            {
                //LogFailure(ipAddress, "No Authorization header found");
                return AuthenticateResult.Fail("No Authorization header found");
            }

            var authHeader = AuthenticationHeaderValue.Parse(authData);
            if (!authHeader.Scheme.Equals("Bearer") && !authHeader.Scheme.Equals("Basic"))
            {
                //LogFailure(ipAddress, $"Unknown scheme [{authHeader.Scheme}]");
                return AuthenticateResult.Fail($"Unknown scheme [{authHeader.Scheme}]");
            }

            //if (IsBlocked(ipAddress))
            //{
            //    return AuthenticateResult.Fail("Too many failed attempts. Try again later.");
            //}

            User? user = null;
            try
            {
                var orgId = ServiceUtils.GetOrgId(Request);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);

                if (authHeader.Scheme.ToUpper().Equals("BASIC"))
                {
                    user = await Task.Run(() => AuthenticateBasic(orgId, credentialBytes, Request));
                }
                else
                {
                    user = await Task.Run(() => AuthenticateBearer(orgId, credentialBytes, Request));
                }
            }
            catch (Exception e)
            {
                //LogFailure(ipAddress, $"Invalid Authorization Header for [{authHeader.Scheme}]");
                return AuthenticateResult.Fail($"Invalid Authorization Header for [{authHeader.Scheme}]");
            }

            if (user == null)
            {
                //LogFailure(ipAddress, $"Invalid username or password for [{authHeader.Scheme}]");
                return AuthenticateResult.Fail($"Invalid username or password for [{authHeader.Scheme}]");
            }

            //ClearFailure(ipAddress);

            var identity = new ClaimsIdentity(user.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Context.Request.Headers.Add("AuthenScheme", Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private void LogFailure(string? ipAddress, string reason)
        {
            if (string.IsNullOrEmpty(ipAddress)) return;

            if (!FailedAttemptsCache.TryGetValue(ipAddress, out int attempts))
            {
                attempts = 0;
            }

            attempts++;
            FailedAttemptsCache.Set(ipAddress, attempts, TimeSpan.FromMinutes(5));

            Log.Warning($"[SECURITY] Failed auth attempt from {ipAddress}. Reason: {reason}. Attempt {attempts}/5");

            if (attempts >= 5)
            {
                Log.Warning($"[SECURITY] Blocking IP {ipAddress} due to excessive failed attempts.");
            }
        }

        private bool IsBlocked(string? ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) return false;
            return FailedAttemptsCache.TryGetValue(ipAddress, out int attempts) && attempts >= 5;
        }

        private void ClearFailure(string? ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) return;
            FailedAttemptsCache.Remove(ipAddress);
        }
    }
}
