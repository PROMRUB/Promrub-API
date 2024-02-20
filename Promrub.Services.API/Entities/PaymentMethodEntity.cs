using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("PaymentMethods")]
    public class PaymentMethodEntity
    {
        [Key]
        [Column("payment_method_id")]
        public Guid? PaymentMethodId { get; set; }

        [Column("payment_method_code")]
        public int? PaymentMethodCode { get; set; }

        [Column("payment_method_name_en")]
        public string? PaymentMethodNameEn { get; set; }

        [Column("payment_method_name_th")]
        public string? PaymentMethodNameTh { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; } 
    }
}
