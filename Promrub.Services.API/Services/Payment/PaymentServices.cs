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
using QuestPDF.Drawing;
using QuestPDF;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;
using Promrub.Services.API.Extensions;

namespace Promrub.Services.API.Services.Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPosRepository posRepository;
        private readonly IPaymentChannelRepository paymentChannelRepository;
        private readonly IPaymentRepository paymentRepository;

        public PaymentServices(IMapper mapper,
            IConfiguration configuration,
            IOrganizationRepository organizationRepository,
            IPosRepository posRepository,
            IPaymentChannelRepository paymentChannelRepository,
            IPaymentRepository paymentRepository)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.organizationRepository = organizationRepository;
            this.posRepository = posRepository;
            this.paymentChannelRepository = paymentChannelRepository;
            this.paymentRepository = paymentRepository;
        }


        private void SetOrgId(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            paymentRepository!.SetCustomOrgId(orgId);
            paymentChannelRepository.SetCustomOrgId(orgId);
            posRepository.SetCustomOrgId(orgId);
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
                CardList = new List<PaymentChannelList>(),
                PaymentStatus = paymentDetails!.PaymentStatus,
                RedirectUrl = org.RedirectUrl!
            };
            return result;
        }

        public async Task<Qr30GenerateResponse> GetPromtPayQrCode(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            var paymentChannelDetails = (await paymentChannelRepository.GetPaymentChannels()).Where(x => x.PaymentChannelType == 2).FirstOrDefault();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            var mode = bool.Parse(configuration["IsDev"]);
            var request = new ScbQr30PaymentRequest
            {
                IsDev = mode,
                PromtRubServices = true,
                Amount = paymentDetails.TotalTransactionPrices.ToString(),
                BillerId = paymentChannelDetails.BillerId,
                TransactionId = paymentDetails.TransactionId
            };
            var result = await paymentRepository.QRGenerate(request);
            return mapper.Map<ScbQrGenerateData, Qr30GenerateResponse>(result.Data!);
        }

        public async Task<MemoryStream> GenerateReceipt(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var pos = posRepository.GetPosByOrg().FirstOrDefault();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            var paymentItems = paymentRepository.GetTransactionItem((Guid)paymentDetails.PaymentTransactionId!).ToList();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            FontManager.RegisterFont(File.OpenRead(Path.Combine("Fonts", "Prompt.ttf")));
            byte[] bytes = Convert.FromBase64String(org.OrgLogo!.Split(",")[1]);
            string brn = org.BrnId == "00000" ? " สำนักงานใหญ่" : " (" + org.BrnId + ")";
            byte[] promptBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABoAAAAaCAYAAACpSkzOAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAARlSURBVHgBnVZdSJtXGD750dhoNNEau4m/2YaMddPB2KhbwW3Mi13sovu5GPVigg4GBmEXgw224c0QNEOGF7vYdrNBN5BuDFqLNaShWFtrsBUL0WBqQxF/Yo2Nf4na5znNkc9PrV974OV83znveZ7393yfSTzFmJqaKkun0x9ub28nbTZbwOPxzBg9azKqODEx0QwSn8VicWxtbQmQ3cvJyTlTU1Nzw8h5sxEleOJOpVK/ZmdnOxYWFsTy8jKJypLJ5N/C4DBEBE/e2tnZkbokWVlZkevwrDIUClUawTBEhLEID8T6+roAoZS1tTW198AIgCEi5OEqptHV1VVRXFwsHA6HXM/Nze2pq6t7dqKWlpYs/ZrJZDoD4L+Qp6Tdbl8A2U8ojO/0et3d3ccOwtxXdT6f712z2fwfQvU/gL5ta2uLqL2urq6yrKysSpBaQBZubm6+r3Cw9w3OfQEZ9Xq9nx1JBIt+BpgXBfDYZbP5R6vVehLzaRAcZ344MrmKQPwwKh+F8WkGIg0jClpbW1e1uFY9Ebxwsk8AymeBXvkezUlCgRJnpWmJPHj3UJcjs2dFLt2Yo08kgnUpHoQXIi8vTxISQJHD210iViINUHuKrLa2NqbH3VcMADhHT0gi44AQKmKS6IXrFJ7JyFhDQ0P6SCJYaEe4pLUkiUajwu/3y2cFqoSNGwwGpY4iglGvoTBOHUkE5a9pKUPAg7hm5G2ABO8jokGRSETk5+dLXYaRgr3P9bh7qq63t9eFKa480oZHAclDCKXKEQ1ikVDoNefNzc04SrzoUI+g9DoBCUAwJRy8fpaWlgQv1bm5OTE/Py82NjZ2ibW6MKKws7PzhBZ7T9VB0UULabk6xMF7jR6wzHEzSC94HS0uLsrriGvaQV1EIudQj0BwS3mjDlAIyrDMzs7u5ofPNIg51BKoRi8qKpo/lAhWTMPaB1QmoRJ1WxcUFMjCSCQSwuVyyTWSKT16mmmHW01NTclDiXBtpDD9qSXiYfYUyZijeDwuZ5Y2Q8nQKQL16YDu70I39t0MONQNi77KdPsYimMZ8+nCwsLdyuJgXlg4GRJ+Ql4FAfH4H3FOj2vRL/T39y81Nja+j8cTPT09Xw4MDNgBer60tPQkgPNUjpjHTDn/29HREUJV9oHoAgyKtbe39z2RqKKi4hTGXFVV1fVAIHAH35+HCNU7+GfwI/n9g4ODt91u95WRkZHfkK/E8PCwD2Q3x8fHbdh/cXJy8nwsFhtFHx0rKSk5jhDHFfaehi0vL/8DVvWhYZ+H8ifIUQLW34aVl0D4HN4ZNy90ovX19XeHhoZYQA/RT2cx/4K9BDx8Cbkbwlr9zMzMDwd6BCs/QhW9ibi/gtdtyD+QMsT/Pay/wV8shO9lECVQDDYk/wUQXIR+HHIW4Qxj7wOIC3IHFTp2YDHwIwfxQMnudDqvwfVcPCcBcBkkTpC8jfePoeOExTa8u8PhcBCRSGE9MD09Hayurg4hIjb8B17WYj8CkP2nL3RAlU0AAAAASUVORK5CYII=");
            byte[] pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.ContinuousSize(63, Unit.Millimetre);
                    page.MarginTop(1, Unit.Millimetre);
                    page.MarginLeft(2, Unit.Millimetre);
                    page.MarginRight(2, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    page.Content()
                        .PaddingVertical(1, Unit.Millimetre)
                        .Column(x =>
                        {
                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignCenter()
                                        .PaddingTop(2, Unit.Millimetre)
                                        .Width(100)
                                        .Image(bytes);
                                });

                            x.Spacing(1);

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignCenter()
                                        .Text("ใบเสร็จรับเงิน/ใบกำกับภาษีอย่างย่อ")
                                        .FontFamily("Prompt");
                                });

                            x.Item()
                                .Text(org.OrgName + brn)
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("TAX ID: " + org.TaxId + " (VAT Included)")
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("POS# " + pos.PosKey)
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("Cashier ID: 01")
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("เลขที่: " + paymentDetails)
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("REF: " + paymentDetails.RefTransactionId)
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("วันที่: " + paymentDetails.ReceiptDate!.Value.ToString("dd/MM/yyyy"))
                                .FontFamily("Prompt");

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("ชิ้น")
                                        .FontFamily("Prompt");

                                    grid.Item(6)
                                        .AlignCenter()
                                        .Text("รายการ")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("หน่วยละ")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("รวมเงิน")
                                        .FontFamily("Prompt");

                                });

                            x.Item()
                                .LineHorizontal(1);

                            foreach (var item in paymentItems)
                            {
                                x.Item()
                                    .Grid(grid =>
                                    {
                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignLeft()
                                            .Text(item.Quantity)
                                            .FontFamily("Prompt");

                                        grid.Item(6)
                                            .AlignLeft()
                                            .Text(item.ItemName)
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(((decimal)item.Price).ToString("N2"))
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(((decimal)item.TotalPrices).ToString("N2"))
                                            .FontFamily("Prompt");

                                    });

                                x.Item()
                                    .Grid(grid =>
                                    {
                                        grid.Columns();
                                        grid.Item(10)
                                            .AlignRight()
                                            .Text(DiscountPercentage((decimal)item.Percentage))
                                            .FontFamily("Prompt");



                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(((decimal)item.TotalDiscount).ToString("N2"))
                                            .FontFamily("Prompt");
                                    });
                            }

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .Text("รายการ: " + paymentDetails.ItemTotal + "          จำนวนชิ้น: " + paymentDetails.QuantityTotal)
                                .FontFamily("Prompt");

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(8)
                                        .AlignLeft()
                                        .Text("รวมเป็นเงิน")
                                        .FontFamily("Prompt");

                                    grid.Item(4)
                                        .AlignRight()
                                        .Text(paymentDetails.TotalTransactionPrices.ToString("N2"))
                                        .FontFamily("Prompt");

                                });

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(8)
                                        .AlignLeft()
                                        .Text("ส่วนลด")
                                        .FontFamily("Prompt");

                                    grid.Item(4)
                                        .AlignRight()
                                        .Text(paymentDetails.TotalDiscount.ToString("N2"))
                                        .FontFamily("Prompt");

                                });

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(8)
                                        .AlignLeft()
                                        .Text("รับชำระด้วย")
                                        .FontFamily("Prompt");

                                    grid.Item(4)
                                        .AlignRight()
                                        .Text("QR Code")
                                        .FontFamily("Prompt");

                                });

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(8)
                                        .AlignLeft()
                                        .Text("รับชำระโดย")
                                        .FontFamily("Prompt");

                                    grid.Item(4)
                                        .AlignRight()
                                        .Text("660009")
                                        .FontFamily("Prompt");

                                });

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .AlignCenter()
                                .Width(13, Unit.Point)
                                .Height(13, Unit.Point)
                                .Image(promptBytes);

                            x.Item()
                                .AlignCenter()
                                .Text("Powered by พร้อมรับ")
                                .FontFamily("Prompt")
                                .Bold();
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
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
            var receiptData = await paymentRepository.ReceiptNumberAsync(paymentDetails!.OrgId, paymentDetails.PosId);
            var receiptNo = "EX" + receiptData.ReceiptDate + "-" + receiptData.Allocated!.Value.ToString("D6");
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

        private string DiscountPercentage(decimal? percentage)
        {
            if (percentage is null)
                return "ส่วนลดรายการนี้";
            else if (percentage == 0)
                return "ส่วนลดรายการนี้";
            else
                return "ส่วนลดรายการนี้ " + percentage + "%";
        }
    }
}
