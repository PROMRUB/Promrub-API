namespace Promrub.Services.API.Interfaces
{
    public interface ICache
    {
        public TModel? GetValue<TModel>(string domain, string key);
        public void SetValue<TModel>(string domain, string key, TModel data, int lifeTimeMin);
    }
}
