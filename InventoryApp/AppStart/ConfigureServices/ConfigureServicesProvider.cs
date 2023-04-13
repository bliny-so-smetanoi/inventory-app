using InventoryApp.DataAccess.Providers.EntityProviders;
using InventoryApp.DataAccess.Providers.Interfaces;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesProvider
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProvider, EntityUserProvider>();

        }
    }
}
