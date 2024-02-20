using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class BaseRepository
    {
        private const string RESERVE_ORG_ID = "axxxxnotdefinedxxxxxxa";

        protected PromrubDbContext? context;
        protected string orgId = RESERVE_ORG_ID;

        public void SetCustomOrgId(string customOrgId)
        {
            orgId = customOrgId;
        }

        public void Commit()
        {
            context!.SaveChanges();
        }
    }
}
