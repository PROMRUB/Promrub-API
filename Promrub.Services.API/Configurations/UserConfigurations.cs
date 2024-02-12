using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.ResponseModels.User;

namespace Promrub.Services.API.Configurations
{
    public class UserConfigurations : Profile
    {
        public UserConfigurations() {
            CreateMap<UserEntity,UserResponse>();
        }
    }
}
