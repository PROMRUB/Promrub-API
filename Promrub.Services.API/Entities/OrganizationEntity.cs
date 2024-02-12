using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

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

        [Column("org_description")]
        public string? OrgDescription { get; set; }

        [Column("org_created_date")]
        public DateTime? OrgCreatedDate { get; set; }

    }
}
