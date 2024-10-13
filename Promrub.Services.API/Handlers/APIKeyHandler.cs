using System.Text;

namespace Promrub.Services.API.Handlers
{
    public static class APIKeyHandler
    {
        public static string GetBasicAPIKey(byte[]? jwtBytes)
        {
            var credentials = Encoding.UTF8.GetString(jwtBytes!).Split(new[] { ':' }, 2);
            var username = credentials[0];
            var password = credentials[1];

            return password;
        }
    }
}
