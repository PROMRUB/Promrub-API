using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.User;
using Promrub.Services.API.Models.ResponseModels.User;

namespace Promrub.Services.API.Configurations
{
    public class UserConfigurations : Profile
    {
        public UserConfigurations()
        {
            CreateMap<UserRequest, UserEntity>()
                .ForMember(dest => dest.UserCreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UserEntity, UserResponse>();
        }
    }
}
