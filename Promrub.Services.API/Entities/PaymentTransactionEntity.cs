using Promrub.Services.API.Models.RequestModels.Payment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("PaymenTransactions")]
    public class PaymentTransactionEntity
    {
        [Key]
        [Column("payment_transaction_id")]
        public Guid? PaymentTransactionId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("pos_id")]
        public string? PosId { get; set; }
        [Column("transaction_id")]
        public string? TransactionId { get; set; }

        [Column("item_total")]
        public int ItemTotal { get; set; }

        [Column("quantity_total")]
        public int QuantityTotal { get; set; }

        [Column("totalItems_prices")]
        public decimal TotalItemsPrices { get; set; }

        [Column("total_discount")]
        public decimal TotalDiscount { get; set; }

        [Column("total_transaction_prices")]
        public decimal TotalTransactionPrices { get; set; }

        [Column("create_at")]
        public DateTime? CreateAt { get; set; }
    }
}
