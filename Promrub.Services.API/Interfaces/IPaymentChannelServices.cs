using Promrub.Services.API.Models.RequestModels.Payment;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentChannelServices
    {
        public void AddPaymentChannel(string orgId, PaymentChannelRequest request);
    }
}
