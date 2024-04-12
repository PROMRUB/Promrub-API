using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("PaymentChannels")]
    public class PaymentChannelEntity
    {
        [Key]
        [Column("payment_channel_id")]
        public Guid? PaymentChannelId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("payment_channel_type")]
        public int? PaymentChannelType { get; set; }

        [Column("bank_code")]
        public int? BankCode { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("is_exist")]
        public bool? IsExist { get; set; }

        [Column("create_at")]
        public DateTime? CreateAt { get; set; }

        [Column("biller_id")]
        public string? BillerId { get; set; }
    }
}
