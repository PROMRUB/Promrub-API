using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;

namespace Promrub.Services.API.Configurations
{
    public class PaymentChannelConfigurations : Profile
    {
        public PaymentChannelConfigurations() {
            CreateMap<PaymentChannelRequest, PaymentChannelEntity>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.IsExist, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<PaymentChannelEntity, PaymentChannelList>();
        }
    }
}
