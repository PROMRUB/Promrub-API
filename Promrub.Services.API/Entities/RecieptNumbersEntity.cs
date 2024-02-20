using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("RecieptNo")]
    public class ReceiptNumbersEntity
    {
        [Key]
        [Column("reciept_id")]
        public Guid? ReceiptId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("reciept_date")]
        public string? ReceiptDate { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
