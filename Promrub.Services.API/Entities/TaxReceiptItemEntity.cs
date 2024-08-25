using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Promrub.Services.API.Entities;

[Table("TaxReceipt")]
public class TaxReceiptEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("tax_id")]
    public Guid TaxId { get; set; }
    public TaxScheduleEntity Tax { get; set; }
    
    [Column("receipt_no")]
    public string ReceiptNo { get; set; }

    // [Column("payment_transaction")]
    // public Guid PaymentTransactionId { get; set; }
    // public PaymentTransactionEntity PaymentTransaction { get; set; }
    public DateTime CreatedDate { get; set; }
}