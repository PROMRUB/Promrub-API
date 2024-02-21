﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("PaymentTransactionItems")]
    public class PaymentTransactionItemEntity
    {
        [Key]
        [Column("payment_transaction_item_id")]
        public Guid? PaymentTransactionItemId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("payment_transaction_id")]
        public Guid? PaymentTransactionId { get; set; }

        [Column("quantity")]
        public int Quantity{ get; set; }

        [Column("item_name")]
        public string? ItemName { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("total_prices")]
        public decimal? TotalPrices { get; set; }
    }
}
