namespace Promrub.Services.API.Models.ResponseModels.Receipt;

public class ReceivePaymentResponse
{
    public Guid Id { get; set; }
    public string PosId { get; set; }
    public string PaymentDateTime { get; set; }
    public string ReceiveNo { get; set; }
    public decimal Amount { get; set; }
    public string PaidBy { get; set; }
    public string InvoiceNo { get; set; }
}