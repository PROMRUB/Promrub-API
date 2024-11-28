using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Promrub.Services.API.Entities
{
    [Table("Coupon")]
    public class CouponEntity
    {
        public CouponEntity()
        {
            CouponId = Guid.NewGuid();
        }

        [Key]
        [Column("coupon_id")]
        public Guid? CouponId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("payment_transaction_id")]
        public Guid? PaymentTransactionId { get; set; }

        [Column("item_name")]
        public string? ItemName { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }
    }
}
