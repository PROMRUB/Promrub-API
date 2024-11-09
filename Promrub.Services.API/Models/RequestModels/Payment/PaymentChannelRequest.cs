using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.RequestModels.Payment
{
    public class PaymentChannelRequest
    {
        public int? PaymentChannelType { get; set; }
        public int? BankCode { get; set; }
        public string? BillerId { get; set; }
    }
}
