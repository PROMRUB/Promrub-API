using Promrub.Services.API.Models.RequestModels.Organization;
using Promrub.Services.API.Models.ResponseModels.Organization;

namespace Promrub.Services.API.Interfaces
{
    public interface IOrganizationService
    {
        public Task<OrganizationResponse> GetOrganization(string orgId);
        public void AddUserToOrganization(string orgId, OrganizationUserRequest user);
        public bool IsUserNameExist(string orgId, string userName);
        public Task<bool> VerifyUserInOrganization(string orgId, string userName);
        public void AddOrganization(string orgId, OrganizationRequest org);
        public Task<List<OrganizationUserResponse>> GetUserAllowedOrganization(string userName);
    }
}
