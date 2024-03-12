using AutoMapper;
using Microsoft.SqlServer.Server;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Utils;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Promrub.Services.API.Enum;
using System.Text;
using SkiaSharp;

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
            var refTransactionId = request.TransactionId;
            SetOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            if (organization is null)
                throw new ArgumentException("1102");
            var transactionId = ServiceUtils.GenerateTransaction(orgId, 16);
            var transactionQuery = mapper.Map<GeneratePaymentTransactionLinkRequestModel, PaymentTransactionEntity>(request);
            transactionQuery.RefTransactionId = refTransactionId;
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
                RefTransactionId = paymentDetails.RefTransactionId,
                OrgName = org.DisplayName,
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

        public async Task<MemoryStream> GenerateReceipt(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            transactionId = "kha43DS11PMJLVMY7KX25670223142909";
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            byte[] pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.ContinuousSize(63, Unit.Millimetre);
                    page.Margin(2, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(18));

                    page.Header()
                        .AlignCenter()
                        .Text("Hello World");

                    page.Content()
                        .PaddingVertical(2, Unit.Millimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
            return new MemoryStream(pdfBytes);
        }

        public async Task<bool> SCBCallback(ScbCallbackRequest request)
        {
            var paymentDetails = paymentRepository.GetTransactionDetailById(request.TransactionId!).FirstOrDefault();
            organizationRepository.SetCustomOrgId(paymentDetails.OrgId!);
            var orgDetail = await organizationRepository.GetOrganization();
            var receiptData = await paymentRepository.ReceiptNumberAsync(paymentDetails!.OrgId);
            var receiptNo = "Abbr.RCP" + receiptData.ReceiptDate + "-" + receiptData.Allocated!.Value.ToString("D4");
            var receiptDate = DateTime.UtcNow;
            string token = string.Empty;
            var receipt = new PaymentTransactionEntity
            {
                TransactionId = request.PromrubReferenceNo,
                ReceiptNo = receiptNo,
                ReceiptDate = receiptDate,
                ReceiptAmount = (decimal?)request.Amount
            };
            await paymentRepository.ReceiptUpdate(receipt);
            var receiptDoc = await GenerateReceipt(paymentDetails.OrgId!, paymentDetails.TransactionId!);
            var bytes = receiptDoc.ToArray();
            string base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
            switch (orgDetail.Security)
            {
                case EnumAuthorizationType.BASIC:
                    var credential = orgDetail.SecurityCredential + ":" + orgDetail.SecurityPassword;
                    var credentialBytes = System.Text.Encoding.UTF8.GetBytes(credential);
                    token = "BASIC " + Convert.ToBase64String(credentialBytes);
                    var result = await paymentRepository.Callback(orgDetail.CallbackUrl!, new OrganizationCallbackRequest(paymentDetails.RefTransactionId!, base64), token);
                    break;
                case EnumAuthorizationType.BEARERE:
                    token = "BEARER ";
                    break;
            }
            return true;
        }
    }
}
