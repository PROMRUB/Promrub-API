using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.ApiKey;
using Promrub.Services.API.Models.ResponseModels.ApiKey;
using Promrub.Services.API.Utils;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Promrub.Services.API.Services.ApiKey
{
    public class ApiKeyService : BaseService, IApiKeyService
    {
        private readonly IApiKeyRepository? repository;
        private DateTime compareDate = DateTime.Now;
        private readonly IMapper mapper;

        public ApiKeyService(IMapper mapper,
            IApiKeyRepository repository) : base()
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public void SetCompareDate(DateTime dtm)
        {
            compareDate = dtm;
        }

        public List<ApiKeyResponse> GetApiKeys(string orgId)
        {
            repository!.SetCustomOrgId(orgId);
            var query = repository!.GetApiKeys();
            return mapper.Map<IEnumerable<ApiKeyEntity>, List<ApiKeyResponse>>(query);
        }

        public async Task<ApiKeyResponse> GetApiKey(string orgId, string apiKey)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetApiKey(apiKey);
            if (query == null)
                throw new KeyNotFoundException("9500");
            return mapper.Map<ApiKeyEntity, ApiKeyResponse>(query);
        }

        public async Task<ApiKeyResponse> VerifyApiKey(string orgId, string apiKey)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetApiKey(apiKey);
            if (query == null)
                throw new KeyNotFoundException("9500");
            else if ((query.KeyExpiredDate != null) && (DateTime.Compare(compareDate, (DateTime)query.KeyExpiredDate!) > 0))
                throw new UnauthorizedAccessException("9500");
            return mapper.Map<ApiKeyEntity, ApiKeyResponse>(query);
        }

        public async void AddApiKey(string orgId, ApiKeyRequest apiKey)
        {
            repository!.SetCustomOrgId(orgId);
            var request = mapper.Map<ApiKeyRequest, ApiKeyEntity>(apiKey);
            repository!.AddApiKey(request);
        }
        public async void Update(string orgId, ApiKeyRequest apiKey, string key)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetApiKey(key);
            if (query == null)
                throw new KeyNotFoundException("9500");
            query.RedirectUrl = apiKey.RedirectUrl;
            repository.UpdateApiKey(query);
        }

        public void DeleteApiKeyById(string orgId, string keyId)
        {
            if (!ServiceUtils.IsGuidValid(keyId))
                throw new KeyNotFoundException($"9500");
            repository!.SetCustomOrgId(orgId);
            repository!.DeleteApiKeyById(keyId);
        }
    }
}
