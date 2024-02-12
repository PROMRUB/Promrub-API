using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace Promrub.Services.API.Entities
{
    [Table("Users")]
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(UserEmail), IsUnique = true)]
    public class UserEntity
    {
        public UserEntity()
        {
            UserId = Guid.NewGuid();
            UserCreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("user_name")]
        public string? UserName { get; set; }

        [Column("user_email")]
        public string? UserEmail { get; set; }

        [Column("user_created_date")]
        public DateTime? UserCreatedDate { get; set; }
    }
}
