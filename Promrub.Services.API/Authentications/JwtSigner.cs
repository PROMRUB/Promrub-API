using Microsoft.IdentityModel.Tokens;
using Promrub.Services.API.Interfaces;

namespace Promrub.Services.API.Authentications
{
    public class JwtSigner : IJwtSigner
    {
        public JwtSigner()
        {
        }

        public static void ResetSigedKeyJson()
        {
            JwtSignerKey.ResetSigedKeyJson();
        }

        public SecurityKey GetSignedKey(string? url)
        {
            return JwtSignerKey.GetSignedKey(url);
        }
    }
}
