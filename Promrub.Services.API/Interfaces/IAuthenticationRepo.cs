using Promrub.Services.API.Models.Authentications;

namespace Promrub.Services.API.Interfaces
{
    public interface IAuthenticationRepo
    {
        public Task<User>? Authenticate(string orgId, string user, string password, HttpRequest request);
    }
}
