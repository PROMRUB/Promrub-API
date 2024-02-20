using Promrub.Services.API.Models.RequestModels.Payment;
using Promrub.Services.API.Models.ResponseModels.Payment;

namespace Promrub.Services.API.Interfaces
{
    public interface IPaymentServices
    {
        public Task<GeneratePaymentLinkModel> GeneratePaymentTransaction(string orgId, GeneratePaymentTransactionLinkRequestModel request);
    }
}
