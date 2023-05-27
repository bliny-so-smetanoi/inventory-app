using InventoryApp.Contracts.Responses;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using InventoryApp.Models.ResultModels;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityClassroomProvider : EntityProvider<ApplicationContext, Classroom, Guid>, IClassroomProvider
    {
        private readonly ApplicationContext _context;
        public EntityClassroomProvider(ApplicationContext context) : base(context) { 
            
            _context = context;
        }

        public async Task<List<SearchResult>> SearchClassroomByQuery(string category, int numberOfItems)
        {
            try
            {
                var result = _context.SearchResults.FromSql<SearchResult>($"select distinct classrooms.*, count(*) as number_of_items from classrooms join items on items.classroom_id = classrooms.id join categories on categories.id = items.category_id where categories.name = {category} group by classrooms.id having count(*) >= {numberOfItems}").ToList();
               
                return result;
            } catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<StatisticsCategoryPerClassResult>> StatisticsPerClassCategory(string classroom)
        {
            try
            {
                var result = _context.StatisticsCategoryPerCat.FromSql<StatisticsCategoryPerClassResult>($"select categories.name, count(items.category_id) from items join categories on categories.id = items.category_id where items.classroom_id = {Guid.Parse(classroom)} group by categories.name").ToList();

                return result;
            } catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<GetClassroomsNameResponse>> GetClassroomsName()
        {
            try
            {
                var result = new List<GetClassroomsNameResponse>();
                var classrooms = await GetAll();

                foreach ( var classroom in classrooms )
                {
                    result.Add(new GetClassroomsNameResponse
                    {
                        Id = classroom.Id,
                        Name = classroom.ClassroomName
                    });
                }

                return result;
            } catch(Exception)
            {
                throw;
            }
        }

        
    }
}
