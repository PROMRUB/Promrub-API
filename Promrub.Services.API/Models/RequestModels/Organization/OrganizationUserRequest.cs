namespace Promrub.Services.API.Models.RequestModels.Organization
{
    public class OrganizationUserRequest
    {
        public Guid? OrgUserId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? RolesList { get; set; }
    }
}
