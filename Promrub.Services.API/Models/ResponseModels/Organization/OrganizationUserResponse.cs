using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.ResponseModels.Organization
{
    public class OrganizationUserResponse
    {
        public Guid? OrgUserId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RolesList { get; set; }
    }
}
