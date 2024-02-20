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
                .ForMember(x => x.OrgId, opt => opt.MapFrom( x => Guid.NewGuid()))
                .ForMember(x => x.OrgCreatedDate, opt => opt.MapFrom( x => DateTime.UtcNow));
            CreateMap<OrganizationEntity, OrganizationResponse>();
        }
    }
}
