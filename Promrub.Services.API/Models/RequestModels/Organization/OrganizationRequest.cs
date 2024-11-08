using Promrub.Services.API.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.RequestModels.Organization
{
    public class OrganizationRequest
    {
        public string? OrgCustomId { get; set; }
        public string? OrgName { get; set; }
        public string? TaxId { get; set; }
        public string? BrnId { get; set; }
        public string? No { get; set; }
        public string? Road { get; set; }
        public int? Provice { get; set; }
        public int? District { get; set; }
        public int? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public string? OrgDescription { get; set; }
        public string? DisplayName { get; set; }
        public string? CallBackUrl { get; set; }
        public string? OrgLogo { get; set; }
        public EnumAuthorizationType Security { get; set; }
        public string? SecurityCredential { get; set; }
        public string? SecurityPassword { get; set; }
        public string? FullAddress { get; set; }
        public string? TelNo { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
    }
}
