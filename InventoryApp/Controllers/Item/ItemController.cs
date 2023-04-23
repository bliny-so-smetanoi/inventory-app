using Amazon.S3.Model.Internal.MarshallTransformations;
using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Item;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace InventoryApp.Controllers.Item
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/item/")]
    [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
    public class ItemController : ControllerBase
    {
        private readonly ItemProvider _itemProvider;
        private readonly IClassroomProvider _classroomProvider;
        public ItemController(ItemProvider itemProvider, IClassroomProvider classroomProvider) { 
            _itemProvider = itemProvider;
            _classroomProvider = classroomProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemCreateParameter itemCreateParameter)
        {
            try
            {
                var newItem = new Models.Item
                {
                    Name= itemCreateParameter.Name,
                    Description= itemCreateParameter.Description,
                    IconUrl= itemCreateParameter.IconUrl,
                    Condition= itemCreateParameter.Condition,
                    ClassroomId= Guid.Parse(itemCreateParameter.ClassroomId),
                    CategoryId= Guid.Parse(itemCreateParameter.CategoryId),
                    ItemNumber= itemCreateParameter.ItemNumber,
                };

                await _itemProvider.Add(newItem);

                return Ok(new { message = "Item was added successfully!" });
            } catch(Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("classroom/{id:guid}")]
        public async Task<IActionResult> GetAllItems(Guid id)
        {
            try
            {
                var result = await _itemProvider.GetAllItems(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byid/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _itemProvider.GetById(id);

                return Ok(result);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ItemCreateParameter parameter)
        {
            try
            {
                var editItem = await _itemProvider.GetById(id) ??
                    throw new Exception("No such item");

                var classroom = await _classroomProvider.FirstOrDefault(x => x.ClassroomName == parameter.ClassroomId);

                editItem.Name = parameter.Name;
                editItem.Description = parameter.Description;
                editItem.IconUrl = parameter.IconUrl;
                editItem.Condition = parameter.Condition;
                editItem.CategoryId = Guid.Parse(parameter.CategoryId);
                editItem.ClassroomId = classroom.Id;
                editItem.ItemNumber = parameter.ItemNumber;

                await _itemProvider.Edit(editItem);

                return Ok(new {message = "Item was edited successfully!"});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var item = await _itemProvider.GetById(id);

                if (item is null) return NotFound();

                await _itemProvider.Remove(item);

                return Ok(new {message = "Item was deleted successfully!"});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
