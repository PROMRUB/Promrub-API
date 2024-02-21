using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.ResponseModels.Role;

namespace Promrub.Services.API.Interfaces
{
    public interface IRoleRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IEnumerable<RoleEntity> GetRolesList(string rolesList);
    }
}
