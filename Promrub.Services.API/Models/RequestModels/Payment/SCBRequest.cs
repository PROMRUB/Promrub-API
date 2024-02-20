namespace Promrub.Services.API.Models.RequestModels.Payment
{
    public class ScbQr30PaymentRequest
    {
        public bool IsDev { get; set; }
        public bool? PromtRubServices { get; set; }
        public string? TransactionId { get; set; }
        public string? Amount { get; set; }
        public string? BillerId { get; set; }
    }
    public class ScbCallbackRequest
    {
        public string? ResultStatus { get; set; }
        public string? ReferenceNo { get; set; }
        public string? PromrubReferenceNo { get; set; }
        public DateTime? PromrubReferenceTime { get; set; }
        public double? Amount { get; set; }
        public string? PromrubResMessage { get; set; }
        public string? TransactionId { get; set; }
    }
}
