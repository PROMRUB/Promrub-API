using Microsoft.AspNetCore.Authorization;
using Promrub.Services.API.Authentications;
using Promrub.Services.API.ExternalServices.Cache;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Seeder;
using Promrub.Services.API.Services.ApiKey;
using Promrub.Services.API.Services.Organization;
using Promrub.Services.API.Services.Payment;
using Promrub.Services.API.Services.Role;
using Promrub.Services.API.Services.User;

namespace Promrub.Services.API.CrossCutting
{
    public static class NativeInjections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<DataSeeder>();

            services.AddHttpClient<IPaymentRepository, PaymentRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IServiceCollection, ServiceCollection>();
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentChannelServices, PaymentChannelServices>();
            services.AddScoped<IPaymentServices, PaymentServices>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentChannelRepository, PaymentChannelRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddTransient<IAuthorizationHandler, GenericRbacHandler>();
            services.AddScoped<IBasicAuthenticationRepo, BasicAuthenticationRepo>();
            services.AddScoped<IBearerAuthenticationRepo, BearerAuthenticationRepo>();
        }
    }
}
