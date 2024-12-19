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

        [Column("api_Key")]
        public string? ApiKey { get; set; }

        [Column("pos_id")]
        public string? PosId { get; set; }

        [Column("ref_transaction_id")]
        public string? RefTransactionId { get; set; }

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

        [Column("receipt_no")]
        public string? ReceiptNo { get; set; }

        [Column("full_receipt_no")]
        public string? FullReceiptNo { get; set; }
        [Column("receipt_date")]
        public DateTime? ReceiptDate { get; set; }

        [Column("receipt_amount")]
        public decimal? ReceiptAmount { get; set; }

        [Column("payment_status")]
        public int? PaymentStatus { get; set; }

        [Column("create_at")]
        public DateTime? CreateAt { get; set; }

        [Column("auth_token")]
        public string? Token { get; set; }

        [Column("is_redirect")]
        public bool? IsRedirect { get; set; }

        [Column("saler")]
        public string? Saler { get; set; }

        [Column("employee_id")]
        public string? EmployeeId { get; set; }

        [Column("customer_tax_id")]
        public Guid? CustomerTaxId { get; set; }

        [Column("biller_id")]
        public string? BillerId { get; set; }
        public CustomerTaxEntity? CustomerTaxEntity { get; set; }
    }
}
