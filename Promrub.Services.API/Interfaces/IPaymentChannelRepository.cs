using Promrub.Services.API.Entities;
using System.Security.Cryptography;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentChannelRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public void AddPaymentChannel(PaymentChannelEntity request);
        public Task<List<PaymentChannelEntity>> GetPaymentChannels();
        public Task<PaymentChannelEntity> UpdateBillerId(Guid paymentChannelId, string billerId);
        public void Commit();
    }
}
