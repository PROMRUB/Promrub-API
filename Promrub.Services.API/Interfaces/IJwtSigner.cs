using Microsoft.IdentityModel.Tokens;

namespace Promrub.Services.API.Interfaces
{
    public interface IJwtSigner
    {
        public SecurityKey GetSignedKey(string? url);
    }
}
