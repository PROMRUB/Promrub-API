using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.Authentications;
using Serilog;
using System.Security.Claims;

namespace Promrub.Services.API.Authentications
{
    public class BearerAuthenticationRepo : IBearerAuthenticationRepo
    {
        private readonly IOrganizationService? organizationService;
        private readonly IUserService? userService;
        public BearerAuthenticationRepo(IOrganizationService organizationService, 
            IUserService? userService)
        {
            this.organizationService = organizationService;
            this.userService = userService;
        }

        private async Task<bool>? VerifyUser(string orgId, string user)
        {
            return await organizationService!.VerifyUserInOrganization(orgId, user);
        }

        public async Task<User>? Authenticate(string orgId, string user, string password, HttpRequest request)
        {
            var verified = await VerifyUser(orgId, user)!;
            if (!verified)
                throw new UnauthorizedAccessException("Cannot Access This Services");

            var result = new User()
            {
                UserName = user,
                Password = "",
                UserId = (await userService!.GetUserByName(orgId, user)).UserId,
                Role = (await organizationService!.GetUserAllowedOrganization(user)).FirstOrDefault()!.RolesList,
                AuthenType = "JWT",
                OrgId = orgId,
            };

            result.Claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()!),
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, result.Role!),
                new Claim(ClaimTypes.AuthenticationMethod, result.AuthenType!),
                new Claim(ClaimTypes.Uri, request.Path),
                new Claim(ClaimTypes.GroupSid, result.OrgId!),
            };

            return result;
        }
    }
}
