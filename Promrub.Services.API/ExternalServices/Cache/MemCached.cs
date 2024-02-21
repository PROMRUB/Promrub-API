using Enyim.Caching;
using Enyim.Caching.Configuration;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Utils;
using Serilog.Sinks.Syslog;

namespace Promrub.Services.API.ExternalServices.Cache
{
    public class MemCached : ICache
    {
        private readonly IMemcachedClient cacheClient;

        public MemCached(IConfiguration cfg)
        {
            var loggerFactory = new LoggerFactory();
            var config = new MemcachedClientConfiguration(loggerFactory, new MemcachedClientOptions());

            var host = ConfigUtils.GetConfig(cfg, "Memcached:host");
            var portStr = ConfigUtils.GetConfig(cfg, "Memcached:port");

            config.AddServer(host, portStr.ToInt());
            cacheClient = new MemcachedClient(loggerFactory, config);
        }

        public TModel? GetValue<TModel>(string domain, string key)
        {
            var lookupKey = $"{domain}:{key}";
            var value = cacheClient.Get<TModel>(lookupKey);

            return value;
        }

        public void SetValue<TModel>(string domain, string key, TModel data, int lifeTimeMin)
        {
            var lookupKey = $"{domain}:{key}";

            var cachedSecond = lifeTimeMin * 60;
            cacheClient.Set(lookupKey, data, cachedSecond);
        }
    }
}
