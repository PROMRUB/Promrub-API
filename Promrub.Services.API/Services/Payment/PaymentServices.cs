using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Utils;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Promrub.Services.API.Services.Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPaymentChannelRepository paymentChannelRepository;
        private readonly IPaymentRepository paymentRepository;
        public PaymentServices(IMapper mapper,
            IConfiguration configuration,
            IOrganizationRepository organizationRepository,
            IPaymentChannelRepository paymentChannelRepository,
            IPaymentRepository paymentRepository)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.organizationRepository = organizationRepository;
            this.paymentChannelRepository = paymentChannelRepository;
            this.paymentRepository = paymentRepository;
        }

        private void SetOrgId(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            paymentRepository!.SetCustomOrgId(orgId);
            paymentChannelRepository.SetCustomOrgId(orgId);
        }

        public async Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId, GeneratePaymentTransactionLinkRequestModel request)
        {
            SetOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            if (organization is null)
                throw new ArgumentException("1102");
            var transactionId = ServiceUtils.GenerateTransaction(orgId, 16);
            var transactionQuery = mapper.Map<GeneratePaymentTransactionLinkRequestModel, PaymentTransactionEntity>(request);
            paymentRepository!.SetCustomOrgId(orgId);
            paymentRepository.AddTransaction(transactionId, transactionQuery);
            var orderList = mapper.Map<List<PaymentTransactionRequestItemList>, List<PaymentTransactionItemEntity>>(request.RequestItemList);
            foreach (var item in orderList)
            {
                item.PaymentTransactionId = transactionQuery.PaymentTransactionId;
                paymentRepository.AddTransactionItem(item);
            }
            paymentRepository.Commit();
            return new GeneratePaymentLinkModel(configuration["PaymentUrl"], orgId, transactionId);
        }

        public async Task<PaymentTransactionDetails> GetPaymentTransactionDetails(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            var promptpatList = mapper.Map<List<PaymentChannelEntity>, List<PaymentChannelList>>(await paymentChannelRepository.GetPaymentChannels());
            var result = new PaymentTransactionDetails()
            {
                OrgName = org.OrgName,
                Prices = paymentDetails!.TotalTransactionPrices,
                HvMobileBanking = org.HvMobileBanking,
                MobileBankingList = new List<PaymentChannelList>(),
                HvPromptPay = org.HvPromptPay,
                PrompayList = promptpatList,
                HvCard = org.HvCard,
                CardList = new List<PaymentChannelList>()
            };
            return result;
        }

        public async Task<Qr30GenerateResponse> GetPromtPayQrCode(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            var mode = bool.Parse(configuration["IsDev"]);
            var request = new ScbQr30PaymentRequest
            {
                IsDev = mode,
                PromtRubServices = true,
                Amount = paymentDetails.TotalTransactionPrices.ToString(),
                BillerId = "010556109879888",
                TransactionId = paymentDetails.TransactionId
            };
            var result = await paymentRepository.QRGenerate(request);
            return mapper.Map<ScbQrGenerateData, Qr30GenerateResponse>(result.Data!);
        }

        public async Task<bool> SCBCallback(ScbCallbackRequest request)
        {
            var paymentDetails = paymentRepository.GetTransactionDetailById(request.TransactionId!).FirstOrDefault();
            var receiptData = await paymentRepository.ReceiptNumberAsync(paymentDetails!.OrgId);
            var receiptNo = "RCP" + receiptData.ReceiptDate + "-" + receiptData.Allocated!.Value.ToString("D4") + "." + paymentDetails.OrgId;
            var receiptDate = DateTime.UtcNow;
            var receipt = new PaymentTransactionEntity
            {
                TransactionId = request.PromrubReferenceNo,
                ReceiptNo = receiptNo,
                ReceiptDate = receiptDate,
                ReceiptAmount = (decimal?)request.Amount
            };
            await paymentRepository.ReceiptUpdate(receipt);
            return true;
        }
    }
}
