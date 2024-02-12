using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.PromServiceDbContext;

namespace Promrub.Services.API.Repositories
{
    public class ApiKeyRepository : BaseRepository, IApiKeyRepository
    {
        public ApiKeyRepository(PromrubDbContext ctx)
        {
            context = ctx;
        }

        public Task<ApiKeyEntity> GetApiKey(string apiKey)
        {
            var result = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId) && x.ApiKey!.Equals(apiKey)).FirstOrDefaultAsync();
            return result!;
        }

        public void AddApiKey(ApiKeyEntity apiKey)
        {
            apiKey.OrgId = orgId;
            context!.ApiKeys!.Add(apiKey);
            context.SaveChanges();
        }

        public void DeleteApiKeyById(string keyId)
        {
            Guid id = Guid.Parse(keyId);

            var query = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId) && x.KeyId.Equals(id)).FirstOrDefault();
            if (query != null)
            {
                context!.ApiKeys!.Remove(query);
                context.SaveChanges();
            }
        }

        public IEnumerable<ApiKeyEntity> GetApiKeys()
        {
            var query = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId)).ToList();
            return query;
        }
    }
}
