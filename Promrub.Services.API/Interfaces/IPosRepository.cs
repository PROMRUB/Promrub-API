using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces
{
    public interface IPosRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IQueryable<PosEntity> GetPosByOrg();
    }
}
