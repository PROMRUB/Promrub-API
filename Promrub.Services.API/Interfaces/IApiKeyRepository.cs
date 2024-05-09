using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.ApiKey;

namespace Promrub.Services.API.Interfaces
{
    public interface IApiKeyRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public Task<ApiKeyEntity> GetApiKey(string apiKey);
        public Task<ApiKeyEntity> GetApiKey(Guid apiKey);
        public void AddApiKey(ApiKeyEntity apiKey);
        public void DeleteApiKeyById(string keyId);
        public IEnumerable<ApiKeyEntity> GetApiKeys();
        public void Commit();
    }
}
