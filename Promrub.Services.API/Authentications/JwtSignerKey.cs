using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Promrub.Services.API.Authentications
{
    public static class JwtSignerKey
    {
        private static string? signedKeyJson = null;

        public static void ResetSigedKeyJson()
        {
            signedKeyJson = null;
        }

        public static SecurityKey GetSignedKey(string? url)
        {
            signedKeyJson = GetSignedKeyJson(url);
            return new JsonWebKey(signedKeyJson);
        }

        public static string GetSignedKeyJson(string? url)
        {
            if (signedKeyJson != null)
            {
                return signedKeyJson;
            }

            Log.Information($"Getting JSON public key from [{url}]");

            var handler = new HttpClientHandler()
            {
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(0.05)
            };

            var task = client.GetAsync(url);
            var response = task.Result;
            signedKeyJson = response.Content.ReadAsStringAsync().Result;

            return signedKeyJson;
        }
    }
}
