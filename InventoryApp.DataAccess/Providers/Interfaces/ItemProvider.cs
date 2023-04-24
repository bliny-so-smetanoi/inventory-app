using InventoryApp.Models;
using static InventoryApp.DataAccess.Providers.EntityProviders.EntityItemProvider;

namespace InventoryApp.DataAccess.Providers.Interfaces
{
    public interface ItemProvider : IProvider<Item, Guid>
    {
        Task<List<object>> GetAllItems(Guid id);
        Task<object> GetOneById(Guid id);
    }
}
