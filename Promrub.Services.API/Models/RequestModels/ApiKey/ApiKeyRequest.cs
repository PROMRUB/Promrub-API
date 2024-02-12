namespace Promrub.Services.API.Models.RequestModels.ApiKey
{
    public class ApiKeyRequest
    {
        public string? ApiKey { get; set; }
        public string? KeyDescription { get; set; }
        public string? RolesList { get; set; }
    }
}
