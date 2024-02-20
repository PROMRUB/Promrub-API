namespace Promrub.Services.API.Models.RequestModels.Payment
{
    public class ScbQr30PaymentRequest
    {
        public bool? PromtRubServices { get; set; }
        public string? TransactionId { get; set; }
        public string? Amount { get; set; }
        public string? BillerId { get; set; }
    }
}
