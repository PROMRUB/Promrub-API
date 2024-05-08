using Promrub.Services.API.Models.RequestModels.ApiKey;
using Promrub.Services.API.Models.ResponseModels.ApiKey;

namespace Promrub.Services.API.Interfaces
{
    public interface IApiKeyService
    {
        public List<ApiKeyResponse> GetApiKeys(string orgId);
        public Task<ApiKeyResponse> GetApiKey(string orgId, string apiKey);
        public Task<ApiKeyResponse> VerifyApiKey(string orgId, string apiKey);
        public void AddApiKey(string orgId, ApiKeyRequest apiKey);
        public void Update(string orgId, ApiKeyRequest apiKey, Guid key);
        public void DeleteApiKeyById(string orgId, string keyId);
    }
}
