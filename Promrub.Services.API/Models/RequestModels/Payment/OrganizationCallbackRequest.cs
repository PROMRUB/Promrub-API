using Promrub.Services.API.Models.ResponseModels.Common;

namespace Promrub.Services.API.Models.RequestModels.Payment
{
    public class OrganizationCallbackRequest
    {
        public OrganizationCallbackRequest(string transactionId, string file) {
            this.Status = new StatusResponseModel();
            this.Status.Code = "200";
            this.Status.Message = "Payment Successful";
            this.Data = new OrganizationCallbackRequestData();
            this.Data.TransactionID = transactionId;
            this.Data.Receipt = file;
        }
        public StatusResponseModel? Status { get; set;}
        public OrganizationCallbackRequestData? Data { get; set; }
    }

    public class OrganizationCallbackRequestData
    {
        public string? TransactionID { get; set; }
        public string? Receipt { get; set; }
    }
}
