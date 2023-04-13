using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesSwagger
    {
        private const string AppTitle = "Inventory App API";
        private static readonly string AppVersion = $"q.0.0";
        private const string SwaggerClientConfig = "/swagger/v1/swagger.json";
        private const string SwaggerAdminConfig = "/swagger/v1-admin/swagger.json";
        private const string SwaggerClientUrl = "api/client/manual";
        private const string SwaggerAdminUrl = "api/admin/manual";

        /// <summary>
        /// ConfigureServices Swagger services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = AppTitle,
                    Version = AppVersion,
                    Description = "Web API for Inventory App Client"
                });

                options.SwaggerDoc("v1-admin", new OpenApiInfo
                {
                    Title = AppTitle,
                    Version = AppVersion,
                    Description = "Web API for Inventory App Admin"
                });
                options.ResolveConflictingActions(x => x.First());

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                options.OperationFilter<ApplySummariesOperationFilter>();
            });
        }

        /// <summary>
        /// Set up some properties for swagger UI for client
        /// </summary>
        /// <param name="settings"></param>
        public static void SwaggerClientSettings(SwaggerUIOptions settings)
        {
            settings.SwaggerEndpoint(SwaggerClientConfig, $"{AppTitle} v.{AppVersion}");
            settings.RoutePrefix = SwaggerClientUrl;
            settings.HeadContent = $"";
            settings.DocumentTitle = $"{AppTitle}";
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelRendering(ModelRendering.Model);
            settings.DefaultModelsExpandDepth(0);
            settings.DocExpansion(DocExpansion.None);
            settings.OAuthClientId("microservice1");
            settings.OAuthScopeSeparator(" ");
            settings.OAuthClientSecret("secret");
            settings.DisplayRequestDuration();
            settings.OAuthAppName("Microservice module API");
        }

        /// <summary>
        /// Set up some properties for swagger UI for client
        /// </summary>
        /// <param name="settings"></param>
        public static void SwaggerAdminSettings(SwaggerUIOptions settings)
        {
            settings.SwaggerEndpoint(SwaggerAdminConfig, $"{AppTitle} v.{AppVersion}-admin");
            settings.RoutePrefix = SwaggerAdminUrl;
            settings.HeadContent = $"";
            settings.DocumentTitle = $"{AppTitle}";
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelRendering(ModelRendering.Model);
            settings.DefaultModelsExpandDepth(0);
            settings.DocExpansion(DocExpansion.None);
            settings.OAuthClientId("microservice1");
            settings.OAuthScopeSeparator(" ");
            settings.OAuthClientSecret("secret");
            settings.DisplayRequestDuration();
            settings.OAuthAppName("Microservice module API");
        }


    }
}
