using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.PromServiceDbContext;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Utils;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IQueryable<PaymentTransactionEntity> GetQuery();
        public PaymentTransactionEntity AddTransaction(string transactionId, PaymentTransactionEntity request);
        public void AddTransactionItem(PaymentTransactionItemEntity request);
        public void AddCouponItem(CouponEntity request);
        public void UpdateBillerId(string transactionId, string billerId);
        public IQueryable<PaymentTransactionEntity> GetTransactionDetail(string transactionId);
        public IQueryable<PaymentTransactionItemEntity> GetTransactionItem(Guid transactionId);
        public IQueryable<CouponEntity> GetTransactionCoupon(Guid transactionId);
        public IQueryable<PaymentTransactionEntity> GetTransactionDetailById(string transactionId);
        public Task<ScbQrGenerateResponse> QRGenerate(ScbQr30PaymentRequest request);
        public Task<ReceiptNumbersEntity> ReceiptNumberAsync(string? orgId, string posId);
        public Task<FullTaxNumberEntity> FullTaxNumberAsync(string? orgId, string? orgCode, string posId, string? branchCode, string cashierCode);
        public Task<PaymentTransactionEntity> ReceiptUpdate(PaymentTransactionEntity request);
        public Task<PaymentTransactionEntity> FullTaxUpdate(PaymentTransactionEntity request);
        public Task<PaymentTransactionEntity> GetPaymentTransaction(string transactionId);
        public Task<PaymentTransactionEntity> ExpireTransaction(PaymentTransactionEntity request);
        public Task<OrganizationCallbackResponse> Callback(string url, OrganizationCallbackRequest request,
            string token);

        public void Commit();

        public DbContext Context();
    }
}