using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities;

[Table("TaxScheduleEntity")]
public class TaxScheduleEntity
{
    [Key]
    [Column("tax_id")]
    public Guid TaxId { get; set; }

    [Column("tax_date")]
    public DateTime TaxDate { get; set; }

    [Column("pos_id")]
    public string? PosId { get; set; }

    [Column("amount")]
    public decimal Amount{ get; set; }

    [Column("vat")]
    public decimal Vat { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }
    
    [Column("total_receipt")]
    public string TotalReceipt { get; set; }
}