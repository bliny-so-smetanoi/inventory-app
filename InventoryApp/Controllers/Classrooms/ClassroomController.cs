using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Classroom;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace InventoryApp.Controllers.Classrooms
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/classroom/")]
    [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin)]
    public class ClassroomController : Controller
    {
        private readonly IClassroomProvider _classroomProvider;

        public ClassroomController(IClassroomProvider classroomProvider) {
            _classroomProvider = classroomProvider;
        }

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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var classroom = await _classroomProvider.GetById(id);

                if (classroom is null) return NotFound();
                
                await _classroomProvider.Remove(classroom);

                return Ok(new {message = "Classroom has been deleted successfully"});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
