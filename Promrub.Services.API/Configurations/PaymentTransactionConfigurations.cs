using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;

namespace Promrub.Services.API.Configurations
{
    public class PaymentTransactionConfigurations : Profile
    {
        public PaymentTransactionConfigurations() {
            CreateMap<GeneratePaymentTransactionLinkRequestModel, PaymentTransactionEntity>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(_ => 1));
            CreateMap<PaymentTransactionRequestItemList,PaymentTransactionItemEntity>();
            CreateMap<ScbQrGenerateData, Qr30GenerateResponse>();
        }
    }
}
