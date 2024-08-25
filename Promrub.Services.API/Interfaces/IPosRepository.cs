using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces
{
    public interface IPosRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IQueryable<PosEntity> GetPosByOrg();
        public IQueryable<PosEntity> GetPosByOrg(Guid orgId);
        public IQueryable<PosEntity> GetPosByOrgCustom(string orgId);
    }
}
