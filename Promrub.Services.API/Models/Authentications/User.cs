using System.Security.Claims;

namespace Promrub.Services.API.Models.Authentications
{
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid();
        }

        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? AuthenType { get; set; }
        public string? OrgId { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
