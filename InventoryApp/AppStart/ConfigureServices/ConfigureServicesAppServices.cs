using InventoryApp.Contracts.Options;
using InventoryApp.DataAccess;
using InventoryApp.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesAppServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                
                options.UseNpgsql(configuration.GetConnectionString("SqlConnection")).UseSnakeCaseNamingConvention();
            }, ServiceLifetime.Scoped);
            /*services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")).UseSnakeCaseNamingConvention();
            });*/
            QuestPDF.Settings.License = LicenseType.Community;
            services.Configure<SecretOption>(configuration.GetSection("Secrets"));
            services.Configure<AwsS3Options>(configuration.GetSection("AwsS3"));

            services.AddScoped<UserAuthenticationService>();
            services.AddScoped<AwsS3FileUploadService>();
            services.AddScoped<ReportGeneratorService>();

            services.AddAuthentication();
            services.AddAuthorization();
        }
    }
}
