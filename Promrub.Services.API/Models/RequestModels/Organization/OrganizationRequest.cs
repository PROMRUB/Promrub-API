namespace Promrub.Services.API.Models.RequestModels.Organization
{
    public class OrganizationRequest
    {
        public Guid? OrgId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? OrgName { get; set; }
    }
}
