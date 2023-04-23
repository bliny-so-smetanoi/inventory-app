using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using InventoryApp.Models.Users.User;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityClassroomProvider : EntityProvider<ApplicationContext, Classroom, Guid>, IClassroomProvider
    {
        public EntityClassroomProvider(ApplicationContext context) : base(context) { }
    }
}
