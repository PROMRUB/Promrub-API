using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Organization;
using Promrub.Services.API.Models.ResponseModels.Organization;

namespace Promrub.Services.API.Configurations
{
    public class OrganizationConfigurations : Profile
    {
        public OrganizationConfigurations()
        {
            CreateMap<OrganizationRequest, OrganizationEntity>()
                .ForMember(dest => dest.OrgId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.OrgCreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<OrganizationEntity, OrganizationResponse>();
        }
    }
}
