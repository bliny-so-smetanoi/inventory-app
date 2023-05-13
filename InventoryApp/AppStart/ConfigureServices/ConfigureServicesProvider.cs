using InventoryApp.DataAccess.Providers.EntityProviders;
using InventoryApp.DataAccess.Providers.Interfaces;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesProvider
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProvider, EntityUserProvider>();
            services.AddScoped<IClassroomProvider, EntityClassroomProvider>();
            services.AddScoped<ICategoryProvider, EntityCategoryProvider>();
            services.AddScoped<ItemProvider, EntityItemProvider>();
            services.AddScoped<IReportProvider, EntityReportsProvider>();
            services.AddScoped<ImageProvider, EntityImageProvider>();
        }
    }
}
