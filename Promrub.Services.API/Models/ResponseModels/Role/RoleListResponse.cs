using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.ResponseModels.Role
{
    public class RoleListResponse
    {
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public DateTime? RoleCreatedDate { get; set; }
        public string? RoleDefinition { get; set; }
        public string? RoleLevel { get; set; }
    }
}
