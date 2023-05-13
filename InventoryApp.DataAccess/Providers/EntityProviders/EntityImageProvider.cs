using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using System.Linq.Expressions;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityImageProvider : EntityProvider<ApplicationContext, Image, Guid>, ImageProvider
    {
        private readonly ApplicationContext _context;

        public EntityImageProvider(ApplicationContext context) : base(context) { 
            _context= context;
        }
    }
}
