using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Promrub.Services.API.Entities
{
    [Table("Provices")]
    public class ProvinceEntity
    {
        [Key]
        [Column("provice_id")]
        public Guid? ProviceId { get; set; }
        [Column("provice_code")]
        public int? ProvinceCode { get; set; }
        [Column("province_name_en")]
        public string? ProvinceNameEn { get; set; }
        [Column("province_name_th")]
        public string? ProvinceNameTh { get; set; }
    }
}
