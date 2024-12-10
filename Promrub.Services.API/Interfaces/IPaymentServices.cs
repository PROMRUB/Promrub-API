using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using System.Runtime.CompilerServices;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentServices
    {
        public Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId, GeneratePaymentTransactionLinkRequestModel request,string key);
        public Task<PaymentTransactionDetails> GetPaymentTransactionDetails(string orgId, string transactionId);
        public Task<Qr30GenerateResponse> GetPromtPayQrCode(string orgId, string transactionId);
        public Task<(MemoryStream, string ReceiptNo)> GenerateReceipt(string orgId, string transactionId,bool isImage);
        public Task<bool> SCBCallback(ScbCallbackRequest request);
    }
}
