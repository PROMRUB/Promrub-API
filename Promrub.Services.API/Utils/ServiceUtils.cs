using System;
using System.Text.RegularExpressions;

namespace Promrub.Services.API.Utils
{
    public static class ServiceUtils
    {
        private static Random random = new Random();

        public static bool IsGuidValid(string guid)
        {
            try
            {
                Guid.Parse(guid);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetOrgId(HttpRequest request)
        {
            var pattern = @"^\/api\/(.+)\/org\/(.+)\/action\/(.+)$";
            var path = request.Path;
            MatchCollection matches = Regex.Matches(path, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

            var orgId = matches[0].Groups[2].Value;

            return orgId;
        }


        public static string GenerateTransaction(string orgId,int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return orgId + new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray()) + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
