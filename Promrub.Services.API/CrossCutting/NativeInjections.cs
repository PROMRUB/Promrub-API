﻿using Microsoft.AspNetCore.Authorization;
using Promrub.Services.API.Authentications;
using Promrub.Services.API.BackgroundService;
using Promrub.Services.API.Controllers.v1;
using Promrub.Services.API.ExternalServices.Cache;
using Promrub.Services.API.Handlers;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Repositories;
using Promrub.Services.API.Seeder;
using Promrub.Services.API.Services;
using Promrub.Services.API.Services.ApiKey;
using Promrub.Services.API.Services.CustomerTax;
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
            services.AddScoped<IReceivePaymentService, ReceivePaymentService>();
            services.AddScoped<ITaxReportService, TaxReportService>();
            services.AddScoped<ITaxReceiptService, TaxReceiptService>();
            services.AddScoped<MasterController.IMasterService, MasterController.MasterService>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPosRepository, PosRepository>();
            services.AddScoped<IPaymentChannelRepository, PaymentChannelRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReceiptRepository, ReceiptScheduleRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<ICustomerTaxRepository, CustomerTaxRepository>();

            services.AddTransient<IAuthorizationHandler, GenericRbacHandler>();
            services.AddScoped<IBasicAuthenticationRepo, BasicAuthenticationRepo>();
            services.AddScoped<IBearerAuthenticationRepo, BearerAuthenticationRepo>();
            
            // services.AddHostedService<Background>();
        }
    }
}
