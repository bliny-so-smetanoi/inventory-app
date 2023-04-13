using InventoryApp.Contracts.Responses;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityUserProvider : EntityProvider<ApplicationContext, User, Guid>, IUserProvider
    {
        public EntityUserProvider(ApplicationContext context) : base(context)
        {
             
        } 
        public async Task<User> GetByEmail(string email)
        {
            return await FirstOrDefault(x => x.Email == email) ?? throw new ArgumentException();
        }
        public async Task<List<GetAdminsResponse>> GetAllAdminsAndModerators()
        {
            var admins = await Get(x => x.Role == UserRole.Moderator || x.Role == UserRole.Admin);

            var response = admins.Select(admin => new GetAdminsResponse
            {
                Id = admin.Id,
                Email = admin.Email,
                FullName= admin.FullName,
                Role = admin.Role 
            }).ToList();

            return response;
        }
    }
}
