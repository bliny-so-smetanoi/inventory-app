using InventoryApp.AppStart.Filters;
using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Classroom;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.RegularExpressions;

namespace InventoryApp.Controllers.Classrooms
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/classroom/")]
    public class ClassroomController : Controller
    {
        private readonly IClassroomProvider _classroomProvider;
        private readonly ItemProvider _itemProvider;
        public ClassroomController(IClassroomProvider classroomProvider,
            ItemProvider itemProvider)
        {
            _classroomProvider = classroomProvider;
            _itemProvider = itemProvider;
        }

        [ServiceFilter(typeof(UserActionAttribute))]
        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassroomCreateParameter classroomCreate)
        {
            try
            {
                var classroom = new Classroom
                {
                    ClassroomName = classroomCreate.ClassroomName,
                    IconUrl = classroomCreate.IconUrl,
                    Description = classroomCreate.ClassroomDescription
                };

                await _classroomProvider.Add(classroom);

                return Ok(new { message = "Classroom has been created successfully!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Classroom with given number/name is already added!" });
            }
        }

        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var classroom = await _classroomProvider.GetById(id);

                return Ok(classroom);
            } catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet]
        public async Task<IActionResult> GetAllClassrooms()
        {
            try
            {
                var result = await _classroomProvider.GetAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(UserActionAttribute))]
        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ClassroomEditParameter classroomEditParameter)
        {
            try
            {
                var checkClass = await _classroomProvider.FirstOrDefault(x =>
                    x.ClassroomName.Equals(classroomEditParameter.ClassroomName));
                var editClassroom = await _classroomProvider.GetById(id) ??
                    throw new Exception("No such class");

                if (checkClass is not null)
                {
                    if (!checkClass.Id.Equals(editClassroom.Id))
                    {
                        return BadRequest("Classroom with current name/number is existing!");
                    }
                }

                editClassroom.ClassroomName = classroomEditParameter.ClassroomName;
                editClassroom.Description = classroomEditParameter.ClassroomDescription;
                editClassroom.IconUrl = classroomEditParameter.IconUrl;

                await _classroomProvider.Edit(editClassroom);

                return Ok(new { message = "Classroom has been edited successfully!"});

            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [ServiceFilter(typeof(UserActionAttribute))]
        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var classroom = await _classroomProvider.GetById(id);

                if (classroom is null) return NotFound();
                
                await _classroomProvider.Remove(classroom);
                var forDelete = await _itemProvider.Get(x => x.ClassroomId == id);
                if (forDelete is null) return Ok();

                await _itemProvider.RemoveRange(forDelete);

                return Ok(new {message = "Classroom has been deleted successfully"});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet("search/{category}/{numberofitems:int}")]
        public async Task<IActionResult> Search(string category, int numberOfItems)
        {
            try
            {
                Console.WriteLine(category);
                var result = await _classroomProvider.SearchClassroomByQuery(category, numberOfItems);

                return Ok(result);
            } catch(Exception)
            {
                return NotFound();
            }
        }
        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet("search/{classroom}")]
        public async Task<IActionResult> SearchByName(string classroom)
        {
            try
            {
                
                var result = await _classroomProvider.Get(x => Regex.IsMatch(x.ClassroomName, classroom));

                return Ok(result);
            } catch(Exception)
            {
                return NotFound();
            }

        }

        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet("getclassroomsname")]
        public async Task<IActionResult> GetNames()
        {
            try
            {
                var result = await _classroomProvider.GetClassroomsName();

                return Ok(result);
            } catch(Exception)
            {
                return NotFound();
            }
        }

        [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
        [HttpGet("stats/{id:guid}")]
        public async Task<IActionResult> GetStats(Guid id)
        {
            try
            {
                var result = await _classroomProvider.StatisticsPerClassCategory(id.ToString());

                return Ok(result);
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
