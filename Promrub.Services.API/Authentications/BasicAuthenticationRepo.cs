using AutoMapper;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.Authentications;
using Promrub.Services.API.Models.ResponseModels.ApiKey;
using System.Security.Claims;

namespace Promrub.Services.API.Authentications
{
    public class BasicAuthenticationRepo : IBasicAuthenticationRepo
    {
        private readonly IApiKeyService? service;

        public BasicAuthenticationRepo(IApiKeyService service)
        {
            this.service = service;
        }

        private async Task<ApiKeyResponse>? VerifyKey(string orgId, string password)
        {
            var m = await service!.VerifyApiKey(orgId, password);
            if (m != null)
            {
                return m;
            }
            return null;
        }

        public async Task<User>? Authenticate(string orgId, string user, string password, HttpRequest request)
        {
            var m = await VerifyKey(orgId, password);
            if (m == null)
            {
                return null;
            }

            var u = new User()
            {
                UserName = user,
                Password = m.ApiKey,
                UserId = m.KeyId,
                Role = m.RolesList,
                AuthenType = "API-KEY",
                OrgId = m.OrgId,
            };

            u.Claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, u.UserId.ToString()!),
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, u.Role!),
                new Claim(ClaimTypes.AuthenticationMethod, u.AuthenType!),
                new Claim(ClaimTypes.Uri, request.Path),
                new Claim(ClaimTypes.GroupSid, u.OrgId!),
            };

            return u;
        }
    }
}
