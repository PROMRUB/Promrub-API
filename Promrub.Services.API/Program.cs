using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Promrub.Services.API.Authentications;
using Promrub.Services.API.CrossCutting;
using Promrub.Services.API.Helpers;
using Promrub.Services.API.Models.ResponseModels.Common;
using Promrub.Services.API.PromServiceDbContext;
using QuestPDF.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using sib_api_v3_sdk.Client;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsDev")))
    throw new ArgumentNullException($"{0} is Null", "IsDev");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Host")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Host");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Database")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Database");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_User")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_User");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Password")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Password");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PaymentUrl")))
    throw new ArgumentNullException($"{0} is Null", "PaymentUrl");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SCBServicesUrl")))
    throw new ArgumentNullException($"{0} is Null", "SCBServicesUrl");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SCBGenerateQRUrl")))
    throw new ArgumentNullException($"{0} is Null", "SCBServicesUrl");

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
Log.Logger = log;

var cfg = builder.Configuration;

QuestPDF.Settings.License = LicenseType.Community;

// Configuration.Default.ApiKey.Add("api-key",
//     Environment.GetEnvironmentVariable("ERP_EMAIL"));


cfg["IsDev"] = Environment.GetEnvironmentVariable("IsDev")!;
cfg["PostgreSQL:Host"] = Environment.GetEnvironmentVariable("PostgreSQL_Host")!;
cfg["PostgreSQL:Database"] = Environment.GetEnvironmentVariable("PostgreSQL_Database")!;
cfg["PostgreSQL:User"] = Environment.GetEnvironmentVariable("PostgreSQL_User")!;
cfg["PostgreSQL:Password"] = Environment.GetEnvironmentVariable("PostgreSQL_Password")!;
cfg["PaymentUrl"] = Environment.GetEnvironmentVariable("PaymentUrl")!;
cfg["SCBServicesUrl"] = Environment.GetEnvironmentVariable("SCBServicesUrl")!;
cfg["SCBGenerateQRUrl"] = Environment.GetEnvironmentVariable("SCBGenerateQRUrl")!;

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connStr =
     $"Host={cfg["PostgreSQL:Host"]}; Database={cfg["PostgreSQL:Database"]}; Username={cfg["PostgreSQL:User"]}; Password={cfg["PostgreSQL:Password"]}";
builder.Services.AddDbContext<PromrubDbContext>(options => options.UseNpgsql(connStr));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning();


builder.Services.AddAuthentication("BasicOrBearer")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerProxy>("BasicOrBearer", null);

builder.Services.AddAuthorization(options => {
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder("BasicOrBearer");
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

    options.AddPolicy("GenericRolePolicy", policy => policy.AddRequirements(new GenericRbacRequirement()));
});

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Promrub API", Version = "v1", Description = "Promrub API Version 1", });

    config.OperationFilter<SwaggerParameterFilters>();
    config.DocumentFilter<SwaggerVersionMapping>();

    config.DocInclusionPredicate((version, desc) =>
    {
        if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
        var versions = methodInfo.DeclaringType!.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
        var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
        version = version.Replace("v", "");
        return versions.Any(v => v.ToString() == version && maps.AsEnumerable().Any(v => v.ToString() == version));
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the Bearer token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

NativeInjections.RegisterServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PromrubDbContext>();
    dbContext.Database.Migrate();

    // var service = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    // service.Seed();
}

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
