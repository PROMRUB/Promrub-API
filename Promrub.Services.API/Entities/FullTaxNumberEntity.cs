using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("FullTaxNo")]
    public class FullTaxNumberEntity
    {
        [Key]
        [Column("reciept_id")]
        public Guid? ReceiptId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }
        [Column("org_code")]
        public string? OrgCode { get; set; }
        [Column("branch_code")]
        public string? BranchCode { get; set; }
        [Column("cashier_code")]
        public string? CashierCode { get; set; }
        [Column("employee_code")]
        public string? EmployeeCode { get; set; }

        [Column("reciept_date")]
        public string? ReceiptDate { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
