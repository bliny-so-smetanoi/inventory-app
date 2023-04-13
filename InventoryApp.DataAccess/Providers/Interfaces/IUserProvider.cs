using InventoryApp.Contracts.Responses;
using InventoryApp.Models.Users.User;

namespace InventoryApp.DataAccess.Providers.Interfaces
{
    public interface IUserProvider : IProvider<User, Guid>
    {
        Task<User> GetByEmail (string email);
        Task<List<GetAdminsResponse>> GetAllAdminsAndModerators();
    }
}
