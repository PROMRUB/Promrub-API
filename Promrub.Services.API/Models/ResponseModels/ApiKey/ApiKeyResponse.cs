using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Models.ResponseModels.ApiKey
{
    public class ApiKeyResponse
    {
        public Guid? KeyId { get; set; }
        public string? ApiKey { get; set; }
        public string? OrgId { get; set; }
        public DateTime? KeyCreatedDate { get; set; }
        public DateTime? KeyExpiredDate { get; set; }
        public string? KeyDescription { get; set; }
        public string? RolesList { get; set; }
    }
}
