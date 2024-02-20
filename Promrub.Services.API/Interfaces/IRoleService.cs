using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.ResponseModels.Role;

namespace Promrub.Services.API.Interfaces
{
    public interface IRoleService
    {
        public List<RoleListResponse> GetRolesList(string orgId, string rolesList);
    }
}
