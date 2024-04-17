using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace Promrub.Services.API.Entities
{
    [Table("Pos")]
    [Index(nameof(PosId), IsUnique = true)]
    public class PosEntity
    {
        [Key]
        [Column("pos_id")]
        public Guid PosId { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("pos_key")]
        public string? PosKey { get; set; }
    }
}
