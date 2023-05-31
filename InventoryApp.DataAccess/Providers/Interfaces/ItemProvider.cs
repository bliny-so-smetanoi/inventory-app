using InventoryApp.Models;
using static InventoryApp.DataAccess.Providers.EntityProviders.EntityItemProvider;

namespace InventoryApp.DataAccess.Providers.Interfaces
{
    public interface ItemProvider : IProvider<Item, Guid>
    {
        Task<List<object>> GetAllItems(Guid id);
        Task<List<object>> GetAllByNumber(string number);
        Task<object> GetOneById(Guid id);
        Task<List<object>> GetAllFromClassroomByCategory(string classroom, string category);
        Task<List<object>> GetAllFromClassroomByName(string classroom, string name);


    }
}
