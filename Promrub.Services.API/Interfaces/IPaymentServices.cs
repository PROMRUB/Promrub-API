using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Runtime.CompilerServices;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentServices
    {
        public Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId, GeneratePaymentTransactionLinkRequestModel request);
        public Task<PaymentTransactionDetails> GetPaymentTransactionDetails(string orgId, string transactionId);
        public Task<Qr30GenerateResponse> GetPromtPayQrCode(string orgId, string transactionId);
        public Task<bool> SCBCallback(ScbCallbackRequest request);
    }
}
