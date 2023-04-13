using InventoryApp.Contracts.Options;
using InventoryApp.DataAccess;
using InventoryApp.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesAppServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("SqlConnection")).UseSnakeCaseNamingConvention();
            });
            /*services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")).UseSnakeCaseNamingConvention();
            });*/

            services.Configure<SecretOption>(configuration.GetSection("Secrets"));
            services.Configure<AwsS3Options>(configuration.GetSection("AwsS3"));

            services.AddScoped<UserAuthenticationService>();
            services.AddScoped<AwsS3FileUploadService>();

            services.AddAuthentication();
            services.AddAuthorization();
        }
    }
}
