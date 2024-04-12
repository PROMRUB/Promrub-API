using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("OrganizationPaymentEntity")]
    public class OrganizationPaymentEntity
    {
        [Key]
        public int OrgPaymentId { get; set; }
        public int OrgId { get; set; }
        public int PaymentChannelId { get; set; }
        public bool IsActive { get; set; }
        public bool IsExist { get; set; }
    }
}
