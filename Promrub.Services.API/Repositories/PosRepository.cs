using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class PosRepository : BaseRepository, IPosRepository
    {
        private readonly PromrubDbContext context;
        
        public PosRepository(PromrubDbContext context)
        {
            this.context = context;
        }
        public  IQueryable<PosEntity> GetPosByOrg()
        {
            return context.Pos!.Where(context => context.OrgId.Equals(orgId));
        }
    }
}
