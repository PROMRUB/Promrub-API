using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.RequestModels.User
{
    public class UserRequest
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
