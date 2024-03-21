using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;
using Promrub.Services.API.Enum;

namespace Promrub.Services.API.Entities
{
    [Table("Organizations")]
    [Index(nameof(OrgCustomId), IsUnique = true)]
    public class OrganizationEntity
    {
        public OrganizationEntity()
        {
            OrgId = Guid.NewGuid();
            OrgCreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("org_custom_id")]
        public string? OrgCustomId { get; set; }

        [Column("org_name")]
        public string? OrgName { get; set; }

        [Column("display_name")]
        public string? DisplayName { get; set; }

        [Column("org_logo")]
        public string? OrgLogo { get; set; }

        [Column("tax_id")]
        public string? TaxId { get; set; }

        [Column("branch_id")]
        public string? BrnId { get; set; }

        [Column("house_no")]
        public string? No { get; set; }

        [Column("road")]
        public string? Road { get; set; }

        [Column("provice")]
        public string? Provice { get; set; }

        [Column("district")]
        public string? District { get; set; }

        [Column("sub_district")]
        public string? SubDistrict { get; set; }

        [Column("post_code")]
        public string? PostCode { get; set; }

        [Column("org_description")]
        public string? OrgDescription { get; set; }
        [Column("hv_mobile_banking")]
        public bool? HvMobileBanking { get; set; }

        [Column("hv_promtpay")]
        public bool? HvPromptPay { get; set; }

        [Column("hv_card")]
        public bool? HvCard { get; set; }

        [Column("authorization_type")]
        public EnumAuthorizationType Security { get; set; }

        [Column("security_credential")]
        public string? SecurityCredential { get; set; }

        [Column("security_password")]

        public string? SecurityPassword { get; set; }

        [Column("callback_url")]

        public string? CallbackUrl { get; set; }

        [Column("redirect_url")]

        public string? RedirectUrl { get; set; }

        [Column("org_created_date")]
        public DateTime? OrgCreatedDate { get; set; }

    }
}
