using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Marvin.StreamExtensions;
using Newtonsoft.Json.Linq;

namespace Promrub.Services.API.Utils
{
    public static class HttpUtils
    {
        public static TimeZoneInfo? BangkokTimeZone { get; }
        public static DateTime Now => TimeZoneInfo.ConvertTime(DateTime.UtcNow, BangkokTimeZone == null ? TimeZoneInfo.Local : BangkokTimeZone);
        static HttpUtils()
        {
            BangkokTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(o => o.Id == "SE Asia Standard Time") == null
                    ? "Asia/Bangkok"
                    : "SE Asia Standard Time");
        }
        public static async Task<TOut> Get<TOut>(HttpClient httpClient, string url,
            IEnumerable<KeyValuePair<string, string>> headers, CancellationToken cancellationToken)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    foreach (var (key, value) in headers)
                    {
                        request.Headers.Add(key, value);
                    }
                }

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();
                    return stream.ReadAndDeserializeFromJson<TOut>();
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(message: ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

        public static async Task<TOut> Post<TOut>(HttpClient httpClient, string url,
            IEnumerable<KeyValuePair<string, string>> headers, CancellationToken cancellationToken,
            string json)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    if (headers != null)
                    {
                        foreach (var (key, value) in headers)
                        {
                            request.Headers.Add(key, value);
                        }
                    }
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await httpClient
                        .SendAsync(request))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        if (typeof(TOut) == typeof(string))
                        {
                            var str = await response.Content.ReadAsStringAsync();

                            return (TOut)(object)str;
                        }

                        var b = stream.ReadAndDeserializeFromJson<TOut>();
                        return b;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(message: ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

        public static async Task<TOut> PostForm<TOut>(HttpClient httpClient, string url,
            IEnumerable<KeyValuePair<string, string>> headers
            , IEnumerable<KeyValuePair<string, string>> form
            , CancellationToken cancellationToken)
        {
            try
            {
                using (var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    url))
                {
                    if (headers != null)
                    {
                        foreach (var (key, value) in headers)
                        {
                            request.Headers.Add(key, value);
                        }
                    }

                    request.Content = new FormUrlEncodedContent(form);

                    using (var response = await httpClient
                        .SendAsync(request))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        var b = stream.ReadAndDeserializeFromJson<TOut>();
                        return b;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(message: ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}
