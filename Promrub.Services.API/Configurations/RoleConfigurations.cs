using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.ResponseModels.Role;

namespace Promrub.Services.API.Configurations
{
    public class RoleConfigurations : Profile
    {
        public RoleConfigurations()
        {
            CreateMap<RoleEntity,RoleListResponse>();
        }
    }
}
