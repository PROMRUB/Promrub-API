using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities;

[Table("ReceiptReceipt")]
public class ReceiptPaymentEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("receipt_id")]
    public Guid ReceiptId { get; set; }
    public ReceiptScheduleEntity Receipt { get; set; }
    
    [Column("receipt_no")]
    public string ReceiptNo { get; set; }
    
}