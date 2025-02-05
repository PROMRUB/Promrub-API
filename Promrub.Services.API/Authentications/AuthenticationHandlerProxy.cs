using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text;
using Promrub.Services.API.Models.Authentications;
using Promrub.Services.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Promrub.Services.API.Authentications
{
    public class AuthenticationHandlerProxy : AuthenticationHandlerProxyBase
    {
        private readonly IBasicAuthenticationRepo? basicAuthenRepo;
        private readonly IBearerAuthenticationRepo? bearerAuthRepo;
        private readonly IConfiguration config;
        private IJwtSigner signer = new JwtSigner();

        public AuthenticationHandlerProxy(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IBasicAuthenticationRepo bsAuthRepo,
            IBearerAuthenticationRepo brAuthRepo,
            IConfiguration cfg,
            ISystemClock clock)
        : base(options, logger, encoder, clock)
        {
            basicAuthenRepo = bsAuthRepo ?? throw new ArgumentNullException(nameof(bsAuthRepo));
            bearerAuthRepo = brAuthRepo ?? throw new ArgumentNullException(nameof(brAuthRepo));
            config = cfg ?? throw new ArgumentNullException(nameof(cfg));
            signer = new JwtSigner();
        }

        public void SetJwtSigner(IJwtSigner sn) => signer = sn ?? throw new ArgumentNullException(nameof(sn));

        protected override async Task<User> AuthenticateBasic(string orgId, byte[]? jwtBytes, HttpRequest request)
        {
            if (jwtBytes == null || jwtBytes.Length == 0)
                throw new ArgumentException("Invalid credentials", nameof(jwtBytes));

            var credentials = Encoding.UTF8.GetString(jwtBytes).Split(':', 2);
            if (credentials.Length != 2)
                throw new FormatException("Invalid basic authentication format");

            var username = credentials[0];
            var password = credentials[1];

            return await basicAuthenRepo!.Authenticate(orgId, username, password, request);
        }

        protected override async Task<User> AuthenticateBearer(string orgId, byte[]? jwtBytes, HttpRequest request)
        {
            if (jwtBytes == null || jwtBytes.Length == 0)
                throw new ArgumentException("Invalid JWT token", nameof(jwtBytes));

            var accessToken = Encoding.UTF8.GetString(jwtBytes);
            var tokenHandler = new JwtSecurityTokenHandler();

            var param = new TokenValidationParameters
            {
                ValidIssuer = config["SSO:issuer"],
                ValidAudience = config["SSO:audience"],
                IssuerSigningKey = signer.GetSignedKey(config["SSO:signedKeyUrl"]),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, param, out SecurityToken validatedToken);
                var jwt = validatedToken as JwtSecurityToken;

                var userName = jwt?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                if (string.IsNullOrEmpty(userName))
                    throw new SecurityTokenException("Invalid token: missing username");

                return await bearerAuthRepo!.Authenticate(orgId, userName, "", request);
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException("JWT validation failed", ex);
            }
        }
    }
}
