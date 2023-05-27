using InventoryApp.Contracts.Responses;
using InventoryApp.Models;
using InventoryApp.Models.ResultModels;

namespace InventoryApp.DataAccess.Providers.Interfaces
{
    public interface IClassroomProvider : IProvider<Classroom, Guid>
    {
        Task<List<SearchResult>> SearchClassroomByQuery(string category, int numberOfItems);
        Task<List<StatisticsCategoryPerClassResult>> StatisticsPerClassCategory(string classroom);
        Task<List<GetClassroomsNameResponse>> GetClassroomsName();
    }
}
