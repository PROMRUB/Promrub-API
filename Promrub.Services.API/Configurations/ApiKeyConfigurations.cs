using AutoMapper;
using PasswordGenerator;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.ApiKey;
using Promrub.Services.API.Models.ResponseModels.ApiKey;

namespace Promrub.Services.API.Configurations
{
    public class ApiKeyConfigurations : Profile
    {
        public ApiKeyConfigurations() {
            CreateMap<ApiKeyEntity, ApiKeyResponse>();
            CreateMap<ApiKeyRequest, ApiKeyEntity>()
                .ForMember(dest => dest.KeyId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.KeyCreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
