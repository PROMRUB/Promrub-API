using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public PaymentTransactionEntity AddTransaction(string transactionId, PaymentTransactionEntity request);
        public void AddTransactionItem(PaymentTransactionItemEntity request);
        public IQueryable<PaymentTransactionEntity> GetTransactionDetail(string transactionId);
        public IQueryable<PaymentTransactionItemEntity> GetTransactionItem(Guid transactionId);
        public IQueryable<PaymentTransactionEntity> GetTransactionDetailById(string transactionId);
        public Task<ScbQrGenerateResponse> QRGenerate(ScbQr30PaymentRequest request);
        public Task<ReceiptNumbersEntity> ReceiptNumberAsync(string? orgId);
        public Task<PaymentTransactionEntity> ReceiptUpdate(PaymentTransactionEntity request);
        public Task<OrganizationCallbackResponse> Callback(string url, OrganizationCallbackRequest request, string token);
        public void Commit();
    }
}
