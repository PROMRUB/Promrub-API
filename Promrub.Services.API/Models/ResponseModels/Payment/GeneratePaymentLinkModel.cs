using System.Security.Policy;

namespace Promrub.Services.API.Models.ResponseModels.Payment
{
    public class GeneratePaymentLinkModel
    {
        public GeneratePaymentLinkModel(string url,string orgId, string transactionId) {
            PaymentUrl = url + $"?org={orgId}&transactionId={transactionId}";
        }
        public string? PaymentUrl { get; set; }
    }
}
