using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.ResponseModels.User
{
    public class UserResponse
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public DateTime? UserCreatedDate { get; set; }
    }
}
