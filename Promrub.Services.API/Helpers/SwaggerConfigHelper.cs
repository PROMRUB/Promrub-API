using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using static Promrub.Services.API.Helpers.SwaggerConfig;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace Promrub.Services.API.Helpers
{
    public class SwaggerConfig
    {
        public static String? QueryStringParam { get; private set; }
        public static String? CustomHeaderParam { get; private set; }
        public static String? AcceptHeaderParam { get; private set; }

        private static VersioningType CurrentVersioningMethod = VersioningType.None;

        public enum VersioningType
        {
            None, CustomHeader, QueryString, AcceptHeader
        }

        public static VersioningType GetCurrentVersioningMethod()
        {
            return CurrentVersioningMethod;
        }

        public static void UseCustomHeaderApiVersion(string parameterName)
        {
            CurrentVersioningMethod = VersioningType.CustomHeader;
            CustomHeaderParam = parameterName;
        }

        public static void UseQueryStringApiVersion()
        {
            QueryStringParam = "api-version";
            CurrentVersioningMethod = VersioningType.QueryString;
        }
        public static void UseQueryStringApiVersion(string parameterName)
        {
            CurrentVersioningMethod = VersioningType.QueryString;
            QueryStringParam = parameterName;
        }
        public static void UseAcceptHeaderApiVersion(String paramName)
        {
            CurrentVersioningMethod = VersioningType.AcceptHeader;
            AcceptHeaderParam = paramName;
        }
    }

    public class SwaggerParameterFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            try
            {
                var maps = context.MethodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToList();
                var version = maps[0].MajorVersion;
                if (GetCurrentVersioningMethod() == VersioningType.CustomHeader && !context.ApiDescription.RelativePath!.Contains("{version}"))
                {
                    operation.Parameters.Add(new OpenApiParameter { Name = SwaggerConfig.CustomHeaderParam, In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                }
                else if (GetCurrentVersioningMethod() == VersioningType.QueryString && !context.ApiDescription.RelativePath!.Contains("{version}"))
                {
                    operation.Parameters.Add(new OpenApiParameter { Name = SwaggerConfig.QueryStringParam, In = ParameterLocation.Query, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                }
                else if (GetCurrentVersioningMethod() == VersioningType.AcceptHeader && !context.ApiDescription.RelativePath!.Contains("{version}"))
                {

                    operation.Parameters.Add(new OpenApiParameter { Name = "Accept", In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString($"application/json;{SwaggerConfig.AcceptHeaderParam}=" + version.ToString()) } });

                }

                var versionParameter = operation.Parameters.Single(p => p.Name == "version");

                if (versionParameter != null)
                {
                    operation.Parameters.Remove(versionParameter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class SwaggerVersionMapping : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathLists = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                pathLists.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = pathLists;
        }
    }
}
