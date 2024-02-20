using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities
{
    [Table("SubDistricts")]
    public class SubDistrictEntity
    {
        [Key]
        [Column("sub_district_id")]
        public Guid? SubDistrictId { get; set; }

        [Column("province_code")]
        public int? ProvinceCode { get; set; }

        [Column("district_code")]
        public int? DistrictCode { get; set; }

        [Column("sub_district_code")]
        public int? SubDistrictCode { get; set; }

        [Column("sub_district_name_en")]
        public string? SubDistrictNameEn { get; set; }

        [Column("sub_district_name_th")]
        public string? SubDistrictNameTh { get; set; }

        [Column("postal_code")]
        public string? PostalCode { get; set; }
    }
}
