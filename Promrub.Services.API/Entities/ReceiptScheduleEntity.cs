using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities;

[Table("ReceiptScheduleEntity")]
public class ReceiptScheduleEntity
{
    [Key]
    [Column("receipt_id")]
    public Guid ReceiptId { get; set; }

    [Column("receipt_date")]
    public DateTime ReceiptDate { get; set; }
    
    [Column("receive_no")]
    public string ReceiveNo { get; set; }

    [Column("pos_id")]
    public string? PosId { get; set; }

    [Column("amount")]
    public decimal Amount{ get; set; }

    public List<ReceiptPaymentEntity> Item{ get; set; }
  
}