using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityReportsProvider : EntityProvider<ApplicationContext, Reports, Guid>, IReportProvider
    {
        public EntityReportsProvider(ApplicationContext context) : base(context) { }
    }
}
