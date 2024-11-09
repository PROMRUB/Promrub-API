using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Payment;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentChannelServices
    {
        public Task<List<PaymentChannelEntity>> GetPaymentChannel(string orgId);
        public void AddPaymentChannel(string orgId, PaymentChannelRequest request);
        public Task UpdateBillerId(string orgId, Guid paymentChannelId, PaymentChannelRequest request);
    }
}
