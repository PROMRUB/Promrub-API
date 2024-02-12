using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(PromrubDbContext context)
        {
            this.context = context;
        }

        public Task<OrganizationEntity> GetOrganization()
        {
            var result = context!.Organizations!
                .Where(x => x.OrgCustomId!.Equals(orgId))
                .FirstOrDefaultAsync();
            return result!;
        }

        public void AddUserToOrganization(OrganizationUserEntity user)
        {
            user.OrgCustomId = orgId;
            context!.OrganizationUsers!.Add(user);
            context.SaveChanges();
        }

        public async Task<IEnumerable<OrganizationUserEntity>> GetUserAllowedOrganizationAsync(string userName)
        {
            var query = await context!.OrganizationUsers!.Where(
                x => x!.UserName!.Equals(userName))
                .OrderByDescending(e => e.OrgCustomId).ToListAsync();
            return  query;
        }

        public bool IsUserNameExist(string userName)
        {
            var count = context!.OrganizationUsers!
                .Where(x => x!.UserName!.Equals(userName) && x!.OrgCustomId!.Equals(orgId))
                .Count();
            return count >= 1;
        }

        public bool IsCustomOrgIdExist(string orgCustomId)
        {
            var count = context!.Organizations!
                .Where(x => x!.OrgCustomId!.Equals(orgCustomId))
                .Count();
            return count >= 1;
        }

        public async Task<OrganizationUserEntity> GetUserInOrganization(string userName)
        {
            var query = await context!.OrganizationUsers!
                .Where(x => x!.UserName!.Equals(userName) && x!.OrgCustomId!.Equals(orgId))
                .FirstOrDefaultAsync();
            return query!;
        }

        public void AddOrganization(OrganizationEntity org)
        {
            context!.Organizations!.Add(org);
            context.SaveChanges();
        }
    }
}
