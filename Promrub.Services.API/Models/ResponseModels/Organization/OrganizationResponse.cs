using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.ResponseModels.Organization
{
    public class OrganizationResponse
    {
        public Guid? OrgId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? OrgName { get; set; }
        public string? OrgDescription { get; set; }
    }
}
