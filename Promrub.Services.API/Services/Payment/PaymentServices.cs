using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;
using Promrub.Services.API.Utils;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Promrub.Services.API.Enum;
using QuestPDF.Drawing;
using QRCoder;

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
        private readonly IApiKeyRepository apiKeyRepository;
        private readonly ICustomerTaxRepository customerTaxRepository; 

        private readonly Dictionary<string, byte[]> _qrCodes = [];

        public PaymentServices(IMapper mapper,
            IConfiguration configuration,
            IOrganizationRepository organizationRepository,
            IPosRepository posRepository,
            IPaymentChannelRepository paymentChannelRepository,
            IPaymentRepository paymentRepository,
            IApiKeyRepository apiKeyRepository,
            ICustomerTaxRepository customerTaxRepository)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.organizationRepository = organizationRepository;
            this.posRepository = posRepository;
            this.paymentChannelRepository = paymentChannelRepository;
            this.paymentRepository = paymentRepository;
            this.apiKeyRepository = apiKeyRepository;
            this.customerTaxRepository = customerTaxRepository;
        }


        private void SetOrgId(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            paymentRepository!.SetCustomOrgId(orgId);
            paymentChannelRepository.SetCustomOrgId(orgId);
            posRepository.SetCustomOrgId(orgId);
            apiKeyRepository.SetCustomOrgId(orgId);
        }

        public async Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId,
            GeneratePaymentTransactionLinkRequestModel request, string key)
        {
            var refTransactionId = request.TransactionId;
            SetOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            if (organization is null)
                throw new ArgumentException("1102");
            var transactionId = ServiceUtils.GenerateTransaction(orgId, 16);
            var transactionQuery =
                mapper.Map<GeneratePaymentTransactionLinkRequestModel, PaymentTransactionEntity>(request);
            transactionQuery.ApiKey = key;
            transactionQuery.RefTransactionId = refTransactionId;
            transactionQuery.Token = request.Token;
            paymentRepository!.SetCustomOrgId(orgId);
            paymentRepository.AddTransaction(transactionId, transactionQuery);
            var orderList =
                mapper.Map<List<PaymentTransactionRequestItemList>, List<PaymentTransactionItemEntity>>(
                    request.RequestItemList).Select((item, index) =>
                    {
                        item.Seq = index + 1;
                        return item;
                    })
                .ToList();
            var couponist =
                mapper.Map<List<PaymentTransactionRequestDiscountList>, List<CouponEntity>>(
                    request.CouponItemList).Select((item, index) =>
                    {
                        return item;
                    })
                .ToList();
            foreach (var item in orderList)
            {
                item.PaymentTransactionId = transactionQuery.PaymentTransactionId;
                paymentRepository.AddTransactionItem(item);
            }

            foreach (var item in couponist)
            {
                item.PaymentTransactionId = transactionQuery.PaymentTransactionId;
                paymentRepository.AddCouponItem(item);
            }

            paymentRepository.Commit();
            return new GeneratePaymentLinkModel(configuration["PaymentUrl"], orgId, transactionId, request.Token ?? "");
        }

        public async Task<PaymentTransactionDetails> GetPaymentTransactionDetails(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            var api = await apiKeyRepository.GetApiKey(paymentDetails.ApiKey!);
            var promptpayList =
                mapper.Map<List<PaymentChannelEntity>, List<PaymentChannelList>>(
                    await paymentChannelRepository.GetPaymentChannels());

            var result = new PaymentTransactionDetails()
            {
                RefTransactionId = paymentDetails.RefTransactionId,
                OrgName = org.DisplayName,
                Prices = paymentDetails!.TotalTransactionPrices,
                HvMobileBanking = org.HvMobileBanking,
                MobileBankingList = new List<PaymentChannelList>(),
                HvPromptPay = org.HvPromptPay,
                PrompayList = promptpayList,
                HvCard = org.HvCard,
                CardList = new List<PaymentChannelList>(),
                PaymentStatus = paymentDetails!.PaymentStatus == 3 ? 1000 : 1101,
            };

            if (paymentDetails.CreateAt.HasValue && (DateTime.Now - paymentDetails.CreateAt.Value).TotalMinutes > 5)
            {
                if (paymentDetails.PaymentStatus != 3)
                {
                    result.PaymentStatus = 1102;
                    if (paymentDetails.PaymentStatus != 4)
                    {
                        paymentDetails.PaymentStatus = 4;
                        await paymentRepository.ExpireTransaction(paymentDetails);
                    }
                }
            }

            var statusCode = result.PaymentStatus;
            result.RedirectUrl =
                $"{api.RedirectUrl!}?transactionId={paymentDetails.RefTransactionId}&status={statusCode}";
            return result;
        }

        public async Task<Qr30GenerateResponse> GetPromtPayQrCode(string orgId, string transactionId)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            var paymentChannelDetails = (await paymentChannelRepository.GetPaymentChannels())
                .Where(x => x.PaymentChannelType == 2).FirstOrDefault();
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

        public async Task<(MemoryStream, string ReceiptNo)> GenerateReceipt(string orgId, string transactionId,bool isImage = false)
        {
            SetOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var pos = posRepository.GetPosByOrg().FirstOrDefault();
            var paymentDetails = paymentRepository.GetTransactionDetail(transactionId).FirstOrDefault();
            var paymentItems = paymentRepository.GetTransactionItem((Guid)paymentDetails.PaymentTransactionId!).OrderBy(x => x.Seq)
                .ToList();
            var couponItems = paymentRepository.GetTransactionCoupon((Guid)paymentDetails.PaymentTransactionId!)
                .ToList();
            if (org is null || paymentDetails is null)
                throw new ArgumentException("1102");
            FontManager.RegisterFont(File.OpenRead(Path.Combine("Fonts", "Prompt.ttf")));
            byte[] bytes = Convert.FromBase64String(org.OrgLogo!.Split(",")[1]);
            string brn = org.BrnId == "00000" ? " สำนักงานใหญ่" : " " + org.BrnId + "";
            byte[] promptBytes = Convert.FromBase64String(
                "iVBORw0KGgoAAAANSUhEUgAAABoAAAAaCAYAAACpSkzOAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAARlSURBVHgBnVZdSJtXGD750dhoNNEau4m/2YaMddPB2KhbwW3Mi13sovu5GPVigg4GBmEXgw224c0QNEOGF7vYdrNBN5BuDFqLNaShWFtrsBUL0WBqQxF/Yo2Nf4na5znNkc9PrV974OV83znveZ7393yfSTzFmJqaKkun0x9ub28nbTZbwOPxzBg9azKqODEx0QwSn8VicWxtbQmQ3cvJyTlTU1Nzw8h5sxEleOJOpVK/ZmdnOxYWFsTy8jKJypLJ5N/C4DBEBE/e2tnZkbokWVlZkevwrDIUClUawTBEhLEID8T6+roAoZS1tTW198AIgCEi5OEqptHV1VVRXFwsHA6HXM/Nze2pq6t7dqKWlpYs/ZrJZDoD4L+Qp6Tdbl8A2U8ojO/0et3d3ccOwtxXdT6f712z2fwfQvU/gL5ta2uLqL2urq6yrKysSpBaQBZubm6+r3Cw9w3OfQEZ9Xq9nx1JBIt+BpgXBfDYZbP5R6vVehLzaRAcZ344MrmKQPwwKh+F8WkGIg0jClpbW1e1uFY9Ebxwsk8AymeBXvkezUlCgRJnpWmJPHj3UJcjs2dFLt2Yo08kgnUpHoQXIi8vTxISQJHD210iViINUHuKrLa2NqbH3VcMADhHT0gi44AQKmKS6IXrFJ7JyFhDQ0P6SCJYaEe4pLUkiUajwu/3y2cFqoSNGwwGpY4iglGvoTBOHUkE5a9pKUPAg7hm5G2ABO8jokGRSETk5+dLXYaRgr3P9bh7qq63t9eFKa480oZHAclDCKXKEQ1ikVDoNefNzc04SrzoUI+g9DoBCUAwJRy8fpaWlgQv1bm5OTE/Py82NjZ2ibW6MKKws7PzhBZ7T9VB0UULabk6xMF7jR6wzHEzSC94HS0uLsrriGvaQV1EIudQj0BwS3mjDlAIyrDMzs7u5ofPNIg51BKoRi8qKpo/lAhWTMPaB1QmoRJ1WxcUFMjCSCQSwuVyyTWSKT16mmmHW01NTclDiXBtpDD9qSXiYfYUyZijeDwuZ5Y2Q8nQKQL16YDu70I39t0MONQNi77KdPsYimMZ8+nCwsLdyuJgXlg4GRJ+Ql4FAfH4H3FOj2vRL/T39y81Nja+j8cTPT09Xw4MDNgBer60tPQkgPNUjpjHTDn/29HREUJV9oHoAgyKtbe39z2RqKKi4hTGXFVV1fVAIHAH35+HCNU7+GfwI/n9g4ODt91u95WRkZHfkK/E8PCwD2Q3x8fHbdh/cXJy8nwsFhtFHx0rKSk5jhDHFfaehi0vL/8DVvWhYZ+H8ifIUQLW34aVl0D4HN4ZNy90ovX19XeHhoZYQA/RT2cx/4K9BDx8Cbkbwlr9zMzMDwd6BCs/QhW9ibi/gtdtyD+QMsT/Pay/wV8shO9lECVQDDYk/wUQXIR+HHIW4Qxj7wOIC3IHFTp2YDHwIwfxQMnudDqvwfVcPCcBcBkkTpC8jfePoeOExTa8u8PhcBCRSGE9MD09Hayurg4hIjb8B17WYj8CkP2nL3RAlU0AAAAASUVORK5CYII=");
            byte[] promptPowered = Convert.FromBase64String("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nPUNfvvsqzg2zBIRERHZDQptREREZJfY1Wuipmxw47EZRcI+dZP8GXMd7/hQPMbie7/NfkeOJWWvQzj69Et44rUPOenCq7wg5oyLr2Ls2RfR1TswHP7EWHr3/Rxz5ldJOewkjjvjInPdwZxy3tf4YMNW+gJxLzDq8QU5+sunmDqZAw49imPOPo9zL/4mjbXzvE+CB0MJ7wmdfY4+nmMv+Zq3ps4++x/C8cedyvGnfJ2jTryIZfd8j8GY471uZRdOnrfiLs68+OscdvypnH7+5eyx11FceNl4/OZae05PIMLG/jCvv7+O08+9hMO/fJpXR405i0OPP4XS+nbWdvu9c0VERER2l0IbERER2SX/MLQx5URjhEIh4uaHPW7P2zQQ8Bbrta9LrR8Me68TfdTvsNEfY9C2m+tsPxs3rINYiKFowlvE2L7y1BUw/ZntgOlsMBz3+vx4YxdRc40Nb/r8Ue+4PX/t5gH7wI/3jfCtWwf4oGfIe8XpPX+QT30hL0zp6wliTvP2Nw+EeW9drzf+gP0kuWmz5Td92G3QtNv9jX0hPt4yMPx9rGQNxVwGzM3Z2tYWcMw8zNh2riIiIiK7S6GNiIiI7BIbYNhAJu79MmzwEo19Ft5093aZYzZkieOP+MCJMzg46IUz9jq7/sxgNOHt2/AlGI4QDQfMtTESbsJrs8eGIsl9n9n2DAzhD0UIhcN093ThH+o358f5eOM6fK5LIG5GDJir7KScv49jA53eSJTufr93zDHzs9U/4PfW37EVCIXYtGWLt+8PhhkcinrnDNnt8Dn+YIjuvgGv+3As9ln7toqb83sHBolpEWIRERH5Aii0ERERkV2i0EahjYiIiIwshTYiIiIiIiIiIqOQQhsRERERERERkVFIoY2IiIiIiIiIyCik0EZEREREREREZBRSaCMiIiIiIiIiMgoptBERERERERERGYUU2oiIiIiIiIiIjEIKbURERERERERERiGFNiIiIiIiIiIio5BCGxERERERERGRUUihjYiIiIiIiIjIKKTQRkRERERERERkFFJoIyIiIiIiIiIyCim0EREREREREREZhRTaiIiIiIiIiIiMQgptRERERERERERGIYU2IiIiIiIiIiKjkEIbEREREREREZFRSKGNiIiIiIiIiMgopNBGRERERERERGTUgf8LavHSCjZNjFAAAAAASUVORK5CYII=");

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://sales1-dev.prom.co.th/?orgId={org.OrgCustomId}&transactionId={paymentDetails.TransactionId}", QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData); 
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            byte[] qrByte = promptBytes = Convert.FromBase64String(qrCodeImageAsBase64);
            IEnumerable<byte[]> pdfBytes = null;

            if (paymentDetails.CustomerTaxId != null)
            {
                var customerDetail = customerTaxRepository.GetCustomerTaxQuery().Where(x => x.Id == paymentDetails.CustomerTaxId).FirstOrDefault();
                pdfBytes = await FullTaxReciept(bytes, org, brn, paymentDetails, paymentItems, pos, promptPowered, qrByte, customerDetail, isImage);
            }
            else
            {
                if(false)
                {
                    pdfBytes = await PosCarbonReciept(bytes, org, brn, paymentDetails, paymentItems, pos, promptPowered, qrByte, isImage);
                }
                else
                {
                    pdfBytes = await NA4Reciept(bytes, org, brn, paymentDetails, paymentItems, couponItems, pos, promptPowered, qrByte, isImage);
                }
            }

            var image = pdfBytes.ToList()[0];
            return (new MemoryStream(image), paymentDetails.ReceiptNo);
        }

        [Obsolete]
        public async Task<IEnumerable<byte[]>> PosCarbonReciept(byte[] bytes, OrganizationEntity org, string brn, PaymentTransactionEntity paymentDetails, List<PaymentTransactionItemEntity> paymentItems, PosEntity pos, byte[] promptBytes, byte[] qrByte, bool isImage)
        {
            var doc = Document.Create(container =>
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
                                .Text("เลขที่: " + paymentDetails.ReceiptNo)
                                .FontFamily("Prompt");

                            x.Item()
                                .Text("REF#: " + paymentDetails.RefTransactionId)
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
                                            .Text(item.Price != null ? ((decimal)item.Price).ToString("N2") : 0)
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.TotalPrices != null ? ((decimal)item.TotalPrices).ToString("N2") : 0)
                                            .FontFamily("Prompt");
                                    });

                                x.Item()
                                    .Grid(grid =>
                                    {
                                        grid.Columns();
                                        grid.Item(10)
                                            .AlignRight()
                                            .Text(item.Percentage != null ? DiscountPercentage((decimal)item.Percentage) : 0)
                                            .FontFamily("Prompt");


                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.TotalDiscount != null ? ((decimal)item.TotalDiscount).ToString("N2") : 0)
                                            .FontFamily("Prompt");
                                    });
                            }

                            x.Item()
                                .LineHorizontal(1);

                            x.Item()
                                .Text("รายการ: " + paymentDetails.ItemTotal + "          จำนวนชิ้น: " +
                                      paymentDetails.QuantityTotal)
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
                        .Text(x => { });
                });
            });

            IEnumerable<byte[]> pdfBytes = null;

            if (isImage)
            {
                pdfBytes = doc.GenerateImages();
            }
            else
            {
                pdfBytes = new List<byte[]>()
                {
                    doc.GeneratePdf()
                };
            }

            return pdfBytes;
        }

        [Obsolete]
        public async Task<IEnumerable<byte[]>> NA4Reciept(byte[] bytes, OrganizationEntity org, string brn, PaymentTransactionEntity paymentDetails, List<PaymentTransactionItemEntity> paymentItems, List<CouponEntity> couponItems, PosEntity pos, byte[] promptBytes, byte[] qrCode,bool isImage)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
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
                                .PaddingTop(2, Unit.Millimetre)
                                .PaddingLeft(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(10)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .Grid(textGrid =>
                                        {
                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text(org.DisplayName)
                                                .Bold()
                                                .FontSize(10)
                                                .FontFamily("Prompt");

                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text(org.FullAddress)
                                                .FontSize(8)
                                                .FontFamily("Prompt"); ;

                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text("โทรศัพท์: " + (org.TelNo ?? "") +
                                                      (org.Website == null ? "" : " URL: " + org.Website) +
                                                      (org.Email == null ? "" : " อีเมล: " + org.Email))
                                                .FontSize(8)
                                                .FontFamily("Prompt");
                                        });

                                    grid.Item(2)
                                        .AlignCenter()
                                        .PaddingRight(8, Unit.Millimetre)
                                        .Image(bytes, ImageScaling.FitWidth);
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .LineHorizontal(1);

                            x.Spacing(1);

                            x.Item()
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignRight()
                                        .Text("ใบเสร็จรับเงิน/ใบกำกับภาษีอย่างย่อ")
                                        .FontSize(12)
                                        .Bold()
                                        .FontFamily("Prompt");
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(6)
                                        .AlignLeft()
                                        .Text($"เลขประจำตัวผู้เสียภาษี: {org.TaxId}")
                                        .Bold()
                                        .FontFamily("Prompt");

                                    grid.Item(6)
                                        .AlignRight()
                                        .Text($"เลขที: {paymentDetails.ReceiptNo}")
                                        .Bold()
                                        .FontFamily("Prompt");
                                });


                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(6)
                                        .AlignLeft()
                                        .Text($"สาขาที่ออกใบกำกับภาษีอย่างย่อ: {brn}")
                                        .Bold()
                                        .FontFamily("Prompt");

                                    grid.Item(6)
                                        .AlignRight()
                                        .Text($"วันที่: {paymentDetails.ReceiptDate!.Value.ToString("dd.MM.yyyy")}")
                                        .Bold()
                                        .FontFamily("Prompt");
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Background("#D9D9D9")
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(1)
                                        .AlignCenter()
                                        .Text("#")
                                        .FontFamily("Prompt");

                                    grid.Item(5)
                                        .AlignCenter()
                                        .Text("รายการสินค้า")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(1)
                                        .AlignCenter()
                                        .Text("จำนวน")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("จำนวนเงิน")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(1)
                                        .AlignCenter()
                                        .Text("ส่วนลด")
                                        .FontFamily("Prompt");


                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("จำนวนเงิน")
                                        .FontFamily("Prompt");
                                });

                            x.Spacing(1);
                            var count = 1;
                            foreach (var item in paymentItems)
                            {
                                x.Item()
                                    .PaddingLeft(8, Unit.Millimetre)
                                    .PaddingRight(8, Unit.Millimetre)
                                    .Grid(grid =>
                                    {
                                        grid.Columns();
                                        grid.Item(1)
                                            .AlignCenter()
                                            .Text(count)
                                            .FontFamily("Prompt");

                                        grid.Item(4)
                                            .AlignLeft()
                                            .Text(string.IsNullOrEmpty(item.ItemCode) ? item.ItemName : item.ItemCode)
                                            .FontFamily("Prompt");

                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.Quantity)
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.TotalPrices != null ? ((decimal)item.TotalPrices).ToString("N2") : 0.00.ToString("N2"))
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(1)
                                            .AlignRight()
                                            .Text(item.TotalDiscount != null ? ((decimal)item.TotalDiscount).ToString("N2") : 0.ToString("N2"))
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.GrandTotal != null ? ((decimal)item.GrandTotal).ToString("N2") : 0.ToString("N2"))
                                            .FontFamily("Prompt");
                                    });

                                x.Item()
                                        .PaddingLeft(8, Unit.Millimetre)
                                        .PaddingRight(8, Unit.Millimetre)
                                        .Grid(grid =>
                                        {
                                            grid.Columns();
                                            grid.Item(1);

                                            grid.Item(4)
                                                .AlignLeft()
                                                .Text(string.IsNullOrEmpty(item.ItemCode) ? "" : item.ItemName)
                                                .FontSize(8)
                                                .FontFamily("Prompt");

                                            grid.Columns();
                                            grid.Item(2)
                                                .AlignRight();

                                            grid.Columns();
                                            grid.Item(2)
                                                .AlignRight()
                                                .Text($"{(item.Price != null ? (decimal)item.Price : 0).ToString("N2")} (ea)")
                                                .FontSize(8);

                                            grid.Columns();
                                            grid.Item(1)
                                                .AlignRight()
                                                .Text($"{(item.Discount != null ? (decimal)item.Discount : 0).ToString("N2")} (ea)")
                                                .FontSize(8);

                                            grid.Columns();
                                            grid.Item(2);
                                        });

                                count++;
                            }

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .LineHorizontal(1);

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {

                                    grid.Columns();
                                    grid.Item(6)
                                            .AlignLeft()
                                            .Text("REF#: " + paymentDetails.RefTransactionId)
                                            .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(6)
                                            .AlignRight()
                                            .Text("รับชำระด้วย")
                                            .FontFamily("Prompt");
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(3)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .Grid(subGrid =>
                                        {
                                            subGrid.Columns();

                                            subGrid.Item(12)
                                                .AlignLeft()
                                                .Text("สแกน QR Code นี้เพื่อ")
                                                .Bold()
                                                .FontFamily("Prompt");

                                            subGrid.Item(12)
                                                .AlignLeft()
                                                .Text("1. ทำใบกำกับภาษีเต็มรูปแบบ")
                                                .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("1. ชำระค่าจอดรถ")
                                            //    .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("2. ทำส่วนลดค่าจอดรถ")
                                            //    .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("4. อื่นๆ")
                                            //    .FontFamily("Prompt");

                                        });

                                    grid.Item(2)
                                        .AlignLeft()
                                        .Image(qrCode);

                                    grid.Item(3);

                                    grid.Item(5);

                                    grid.Item(4)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .AlignRight()
                                        .Grid(subGrid =>
                                        {

                                            subGrid.Columns();
                                            subGrid.Item(12)
                                                    .Grid(minGrid =>
                                                    {

                                                        minGrid.Columns();
                                                        minGrid.Item(5)
                                                            .AlignLeft()
                                                            .Text("QR30: ")
                                                            .FontFamily("Prompt");

                                                        minGrid.Columns();
                                                        minGrid.Item(7)
                                                            .AlignRight()
                                                            .Text(paymentDetails.TotalTransactionPrices.ToString("N2"))
                                                            .FontFamily("Prompt");
                                                    });

                                            subGrid.Item(12)
                                                    .LineHorizontal(1);

                                            foreach (var item in couponItems)
                                            {
                                                subGrid.Item(12)
                                                       .Grid(minGrid =>
                                                       {

                                                           minGrid.Columns();
                                                           minGrid.Item(5)
                                                               .AlignLeft()
                                                               .Text(item.ItemName)
                                                               .FontFamily("Prompt");

                                                           minGrid.Columns();
                                                           minGrid.Item(7)
                                                               .AlignRight()
                                                               .Text(item.Price?.ToString("N2"))
                                                               .FontFamily("Prompt");
                                                       });
                                            }

                                            subGrid.Item(12)
                                                    .LineHorizontal(2);

                                            subGrid.Spacing(2);

                                            subGrid.Item(12)
                                                    .Text("รับชำระด้วย: QR Code Tag 30")
                                                    .FontFamily("Prompt");

                                            subGrid.Item(12)
                                                    .Text($"รับชำระโดย: {paymentDetails.Saler}")
                                                    .FontFamily("Prompt");
                                        });
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Column(x =>
                        {
                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignCenter()
                                        .Image(promptBytes);
                                });
                        });

                });
            });

            IEnumerable<byte[]> pdfBytes = null;

            if (isImage)
            {
                pdfBytes = doc.GenerateImages();
            }
            else
            {
                pdfBytes = new List<byte[]>()
                {
                    doc.GeneratePdf()
                };
            }

            return pdfBytes;
        }

        [Obsolete]
        public async Task<IEnumerable<byte[]>> FullTaxReciept(byte[] bytes, OrganizationEntity org, string brn, PaymentTransactionEntity paymentDetails, List<PaymentTransactionItemEntity> paymentItems, PosEntity pos, byte[] promptBytes, byte[] qrCode, CustomerTaxEntity custDetail, bool isImage)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
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
                                .PaddingTop(2, Unit.Millimetre)
                                .PaddingLeft(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(10)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .Grid(textGrid =>
                                        {
                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text(org.DisplayName)
                                                .Bold()
                                                .FontSize(10)
                                                .FontFamily("Prompt");

                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text(org.FullAddress)
                                                .FontSize(8)
                                                .FontFamily("Prompt"); ;

                                            textGrid.Columns();
                                            textGrid.Item(10)
                                                .AlignLeft()
                                                .Text("โทรศัพท์: " + (org.TelNo ?? "") +
                                                      (org.Website == null ? "" : " URL: " + org.Website) +
                                                      (org.Email == null ? "" : " อีเมล: " + org.Email))
                                                .FontSize(8)
                                                .FontFamily("Prompt");
                                        });

                                    grid.Item(2)
                                        .AlignCenter()
                                        .PaddingRight(8, Unit.Millimetre)
                                        .Image(bytes, ImageScaling.FitWidth);
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .LineHorizontal(1);

                            x.Spacing(1);

                            x.Item()
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignRight()
                                        .Text("ต้นฉบับใบกำกับภาษี")
                                        .FontSize(12)
                                        .Bold()
                                        .FontFamily("Prompt");
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(6)
                                        .AlignLeft()
                                        .Text($"เลขประจำตัวผู้เสียภาษี: {org.TaxId}")
                                        .Bold()
                                        .FontFamily("Prompt");

                                    grid.Item(6)
                                        .AlignRight()
                                        .Text($"เลขที: {paymentDetails.ReceiptNo}")
                                        .Bold()
                                        .FontFamily("Prompt");
                                });


                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();

                                    grid.Item(6)
                                        .AlignLeft()
                                        .Text($"สาขาที่ออกใบกำกับภาษีอย่างย่อ: {brn}")
                                        .Bold()
                                        .FontFamily("Prompt");

                                    grid.Item(6)
                                        .AlignRight()
                                        .Text($"วันที่: {paymentDetails.ReceiptDate!.Value.ToString("dd.MM.yyyy")}")
                                        .Bold()
                                        .FontFamily("Prompt");
                                });

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Background("#D9D9D9")
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(1)
                                        .AlignCenter()
                                        .Text("#")
                                        .FontFamily("Prompt");

                                    grid.Item(5)
                                        .AlignCenter()
                                        .Text("รายการสินค้า")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("จำนวน")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("ราคา/หน่วย")
                                        .FontFamily("Prompt");

                                    grid.Columns();
                                    grid.Item(2)
                                        .AlignCenter()
                                        .Text("จำนวนเงิน")
                                        .FontFamily("Prompt");
                                });

                            x.Spacing(1);
                            var count = 1;
                            foreach (var item in paymentItems)
                            {
                                x.Item()
                                    .PaddingLeft(8, Unit.Millimetre)
                                    .PaddingRight(8, Unit.Millimetre)
                                    .Grid(grid =>
                                    {
                                        grid.Columns();
                                        grid.Item(1)
                                            .AlignCenter()
                                            .Text(count)
                                            .FontFamily("Prompt");

                                        grid.Item(5)
                                            .AlignLeft()
                                            .Text(string.IsNullOrEmpty(item.ItemCode) ? item.ItemName : item.ItemCode)
                                            .FontFamily("Prompt");

                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.Quantity)
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.Price != null ? ((decimal)item.Price).ToString("N2") : 0)
                                            .FontFamily("Prompt");

                                        grid.Columns();
                                        grid.Item(2)
                                            .AlignRight()
                                            .Text(item.TotalPrices != null ? ((decimal)item.TotalPrices).ToString("N2") : 0)
                                            .FontFamily("Prompt");
                                    });

                                if (!string.IsNullOrEmpty(item.ItemCode) && string.IsNullOrEmpty(item.ItemName))
                                {
                                    x.Item()
                                        .PaddingLeft(8, Unit.Millimetre)
                                        .PaddingRight(8, Unit.Millimetre)
                                        .Grid(grid =>
                                        {
                                            grid.Columns();
                                            grid.Item(1);

                                            grid.Item(5)
                                                .AlignLeft()
                                                .Text(item.ItemName)
                                                .FontFamily("Prompt");

                                            grid.Columns();
                                            grid.Item(2)
                                                .AlignRight();

                                            grid.Columns();
                                            grid.Item(2);

                                            grid.Columns();
                                            grid.Item(2);
                                        });
                                }
                                count++;
                            }

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .LineHorizontal(1);

                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(3)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .Grid(subGrid =>
                                        {
                                            subGrid.Columns();

                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("สแกน QR Code นี้เพื่อ")
                                            //    .Bold()
                                            //    .FontFamily("Prompt");

                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("1. ทำใบกำกับภาษีเต็มรูปแบบ")
                                            //    .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("1. ชำระค่าจอดรถ")
                                            //    .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("2. ทำส่วนลดค่าจอดรถ")
                                            //    .FontFamily("Prompt");

                                            //subGrid.Columns();
                                            //subGrid.Item(12)
                                            //    .AlignLeft()
                                            //    .Text("4. อื่นๆ")
                                            //    .FontFamily("Prompt");

                                        });

                                    grid.Item(2);

                                    grid.Item(3);

                                    grid.Item(4)
                                        .PaddingTop(2, Unit.Millimetre)
                                        .AlignRight()
                                        .Grid(subGrid =>
                                        {

                                            subGrid.Columns();
                                            subGrid.Item(12)
                                                    .Grid(minGrid =>
                                                    {

                                                        minGrid.Columns();
                                                        minGrid.Item(4)
                                                            .AlignLeft()
                                                            .Text("รวมเงินทั้งหมด: ")
                                                            .FontFamily("Prompt");

                                                        minGrid.Columns();
                                                        minGrid.Item(7)
                                                            .AlignRight()
                                                            .Text(paymentDetails.TotalTransactionPrices.ToString("N2"))
                                                            .FontFamily("Prompt");

                                                        minGrid.Columns();
                                                        minGrid.Item(1)
                                                            .AlignRight()
                                                            .Text("บาท")
                                                            .FontFamily("Prompt");
                                                    });

                                            subGrid.Item(12)
                                                    .LineHorizontal(2);

                                            subGrid.Spacing(2);

                                            subGrid.Item(12)
                                                    .Text("รับชำระด้วย: QR Code Tag 30")
                                                    .FontFamily("Prompt");

                                            subGrid.Item(12)
                                                    .Text($"รับชำระโดย: {paymentDetails.Saler}")
                                                    .FontFamily("Prompt");
                                        });
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Column(x =>
                        {
                            x.Item()
                                .PaddingLeft(8, Unit.Millimetre)
                                .PaddingRight(8, Unit.Millimetre)
                                .Grid(grid =>
                                {
                                    grid.Columns();
                                    grid.Item(12)
                                        .AlignCenter()
                                        .Image(promptBytes);
                                });
                        });

                });
            });

            IEnumerable<byte[]> pdfBytes = null;

            if (isImage)
            {
                pdfBytes = doc.GenerateImages();
            }
            else
            {
                pdfBytes = new List<byte[]>()
                {
                    doc.GeneratePdf()
                };
            }

            return pdfBytes;
        }

        public async Task<bool> SCBCallback(ScbCallbackRequest request)
        {
            var paymentDetails = paymentRepository.GetTransactionDetailById(request.TransactionId!).FirstOrDefault();
            organizationRepository.SetCustomOrgId(paymentDetails!.OrgId!);
            var orgDetail = await organizationRepository.GetOrganization();
            var receiptData = await paymentRepository.ReceiptNumberAsync(paymentDetails!.OrgId, paymentDetails!.PosId);
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
            var receiptDoc = await GenerateReceipt(paymentDetails.OrgId!, paymentDetails.TransactionId!,false);
            var bytes = receiptDoc.Item1.ToArray();
            string base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
            switch (orgDetail.Security)
            {
                case EnumAuthorizationType.BASIC:
                    var credential = orgDetail.SecurityCredential + ":" + orgDetail.SecurityPassword;
                    var credentialBytes = System.Text.Encoding.UTF8.GetBytes(credential);
                    token = "BASIC " + Convert.ToBase64String(credentialBytes);
                    var result = await paymentRepository.Callback(orgDetail.CallbackUrl!,
                        new OrganizationCallbackRequest(paymentDetails.RefTransactionId!, base64), token);
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