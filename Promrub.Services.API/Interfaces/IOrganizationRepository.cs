using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces
{
    public interface IOrganizationRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public Task<OrganizationEntity> GetOrganization();
        public void AddUserToOrganization(OrganizationUserEntity user);
        public bool IsUserNameExist(string userName);
        public bool IsCustomOrgIdExist(string orgCustomId);
        public Task<OrganizationUserEntity> GetUserInOrganization(string userName);
        public void AddOrganization(OrganizationEntity org);
        public void UpdateOrganization(OrganizationEntity org);
        public Task<IEnumerable<OrganizationUserEntity>> GetUserAllowedOrganizationAsync(string userName);
    }
}
