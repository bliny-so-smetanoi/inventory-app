using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityCategoryProvider : EntityProvider<ApplicationContext, Category, Guid>, ICategoryProvider
    {
        public EntityCategoryProvider(ApplicationContext context) : base(context) { }
    }
}
