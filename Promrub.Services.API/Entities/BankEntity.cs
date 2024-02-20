using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("Bank")]
    public class BankEntity
    {
        [Key]
        [Column("bank_id")]
        public Guid? BankId { get; set; }

        [Column("bank_code")]
        public int? BankCode { get; set; }

        [Column("bank_abbr")]
        public string? BankAbbr { get; set; }

        [Column("bank_name_en")]
        public string? BankNameEn { get; set; }

        [Column("bank_name_th")]
        public string? BankNameTh { get; set; }

        [Column("bank_swift_code")]

        public string? BankSwiftCode {get; set;}

        [Column("is_active")]
        public bool? IsActive { get; set; }
    }
}
