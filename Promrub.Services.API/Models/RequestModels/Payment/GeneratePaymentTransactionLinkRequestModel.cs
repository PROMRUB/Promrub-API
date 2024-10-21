using System.ComponentModel.DataAnnotations;

namespace Promrub.Services.API.Models.RequestModels.Payment
{
    public class GeneratePaymentTransactionLinkRequestModel
    {
        public string? PosId { get; set; }
        public string? TransactionId { get; set; }
        public List<PaymentTransactionRequestItemList> RequestItemList { get; set; } = new List<PaymentTransactionRequestItemList>();
        public int ItemTotal { get; set; }
        public int QuantityTotal { get; set; }
        public decimal TotalItemsPrices { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTransactionPrices { get; set; }
        public string? Token { get; set; }
    }

    public class PaymentTransactionRequestItemList
    {
        public int Quantity { get; set; } = 0;
        public string? ItemName { get; set; } = string.Empty;
        public decimal? Price { get; set; } = 0;
        public decimal? TotalPrices { get; set; } = 0;
        public decimal? Percentage { get; set; } = 0;
        public decimal? TotalDiscount { get; set; } = 0;
    }
}
