using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(PromrubDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<RoleEntity> GetRolesList(string rolesList)
        {
            var list = rolesList.Split(',').ToList();
            var arr = context!.Roles!.Where(p => list.Contains(p.RoleName!)).ToList();
            return arr;
        }
    }
}
