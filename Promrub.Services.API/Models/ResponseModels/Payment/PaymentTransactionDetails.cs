namespace Promrub.Services.API.Models.ResponseModels.Payment
{
    public class PaymentTransactionDetails
    {
        public string? OrgName { get; set; }
        public decimal? Prices { get; set; }
        public bool? HvMobileBanking { get; set; }
        public List<PaymentChannelList> MobileBankingList {get; set;}
        public bool? HvPromptPay { get; set; }
        public List<PaymentChannelList> PrompayList { get; set; }
        public bool? HvCard { get; set; }
        public List<PaymentChannelList> CardList { get; set; }
    }

    public class PaymentChannelList
    {
        public int? PaymentChannelType { get; set; }
        public int? BankCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Qr30GenerateResponse
    {
        public string? QrRawData { get; set; }
        public string? QrImage { get; set; }
    }
}
