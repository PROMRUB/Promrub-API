using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.ResponseModels.Role;

namespace Promrub.Services.API.Services.Role
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IMapper mapper;
        private readonly IRoleRepository? repository;

        public RoleService(IMapper mapper,
            IRoleRepository repository) : base()
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public List<RoleListResponse> GetRolesList(string orgId, string rolesList)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.GetRolesList(rolesList).ToList();
            return mapper.Map<List<RoleEntity>, List<RoleListResponse>>(result);
        }
    }
}
