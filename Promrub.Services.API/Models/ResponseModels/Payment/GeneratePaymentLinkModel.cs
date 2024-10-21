using System.Security.Policy;

namespace Promrub.Services.API.Models.ResponseModels.Payment
{
    public class GeneratePaymentLinkModel(string url,string orgId, string transactionId,string token)
    {
        public string? PaymentUrl { get; set; } = url + $"?orgId={orgId}&transactionId={transactionId}&token={token}";
    }
}
