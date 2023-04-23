using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityItemProvider : EntityProvider<ApplicationContext, Item, Guid>, ItemProvider
    {
        private readonly ApplicationContext _context;
        public EntityItemProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<object>> GetAllItems(Guid id)
        {
            try
            {
                Console.WriteLine(id);
                var result = (from i in _context.Items
                              join cl in _context.Classrooms
                              on i.ClassroomId equals cl.Id
                              join c in _context.Categories
                              on i.CategoryId equals c.Id
                              where i.ClassroomId.Equals(id)
                              select new
                              {
                                  Name = i.Name,
                                  Id = i.Id,
                                  CategoryName = c.Name,
                                  CategoryId = c.Id,
                                  Description = i.Description,
                                  ItemNumber = i.ItemNumber,
                                  ClassroomId = cl.Id,
                                  Condition = i.Condition,
                                  IconUrl = i.IconUrl,
                                  ClassroomName = cl.ClassroomName,
                              });

                return result.ToList<object>();
            }
            catch (Exception)
            {
                throw;
            }
        }

    
    }
}
