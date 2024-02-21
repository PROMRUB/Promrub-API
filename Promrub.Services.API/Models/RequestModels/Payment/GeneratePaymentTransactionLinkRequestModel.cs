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
    }

    public class PaymentTransactionRequestItemList
    {
        public int Quantity { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrices { get; set; }
    }
}
