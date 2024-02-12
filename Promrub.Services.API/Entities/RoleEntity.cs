using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace Promrub.Services.API.Entities
{
    [Table("Roles")]
    [Index(nameof(RoleName), IsUnique = true)]
    public class RoleEntity
    {
        public RoleEntity()
        {
            RoleId = Guid.NewGuid();
            RoleCreatedDate = DateTime.UtcNow;
            RoleLevel = "";
        }

        [Key]
        [Column("role_id")]
        public Guid? RoleId { get; set; }

        [Column("role_name")]
        public string? RoleName { get; set; }

        [Column("role_description")]
        public string? RoleDescription { get; set; }

        [Column("role_created_date")]
        public DateTime? RoleCreatedDate { get; set; }

        [Column("role_definition")]
        public string? RoleDefinition { get; set; }

        [Column("role_level")]
        public string RoleLevel { get; set; }
    }
}
