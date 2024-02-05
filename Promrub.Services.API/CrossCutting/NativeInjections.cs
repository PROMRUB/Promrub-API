using Microsoft.AspNetCore.Authorization;
using Promrub.Services.API.Handlers;

namespace Promrub.Services.API.CrossCutting
{
    public static class NativeInjections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IServiceCollection, ServiceCollection>();
        }
    }
}
