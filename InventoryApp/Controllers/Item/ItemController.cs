using Amazon.S3.Model.Internal.MarshallTransformations;
using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Item;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;
using InventoryApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace InventoryApp.Controllers.Item
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/item/")]
    [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
    public class ItemController : ControllerBase
    {
        private readonly ItemProvider _itemProvider;
        private readonly IClassroomProvider _classroomProvider;
        private readonly AwsS3FileUploadService _uploadService;
        public ItemController(ItemProvider itemProvider, IClassroomProvider classroomProvider, AwsS3FileUploadService uploadService) { 
            _itemProvider = itemProvider;
            _classroomProvider = classroomProvider;
            _uploadService = uploadService;
        }
        [HttpGet("bynumber/{number}")]
        public async Task<IActionResult> GetByNumber(string number)
        {
            try
            {
                var item = await _itemProvider.Get(x => x.ItemNumber.Equals(number));

                return Ok(item);
            } catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("fromscanner")]
        public async Task<IActionResult> CreateFromScanner([FromBody] ItemCreateParameter itemCreateParameter)
        {
            try
            {
                var classroom = await _classroomProvider.FirstOrDefault(x => x.ClassroomName == itemCreateParameter.ClassroomId);

                if (classroom is null)
                    return NotFound(new { message = "There is no classes with given name or number!" });

                var newItem = new Models.Item
                {
                    Name = itemCreateParameter.Name,
                    Description = itemCreateParameter.Description,
                    IconUrl = itemCreateParameter.IconUrl,
                    Condition = itemCreateParameter.Condition,
                    ClassroomId = classroom.Id,
                    CategoryId = Guid.Parse(itemCreateParameter.CategoryId),
                    ItemNumber = itemCreateParameter.ItemNumber,
                };

                await _itemProvider.Add(newItem);

                return Ok(new { message = "Item was added successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var result = await _uploadService.UploadFileAsync(file);

                return Ok(new { fileUrl = result });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
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
                var result = await _itemProvider.GetOneById(id);

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

                if (classroom is null)
                    return NotFound(new { message = "There is no classes with given name or number!" });

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

        [HttpGet("search/{classroom}/{param}")]
        public async Task<IActionResult> Search(string classroom, string param)
        {
            try
            { 
                var result = await _itemProvider.Get(x => (Regex.IsMatch(x.Name, param) && x.ClassroomId.Equals(Guid.Parse(classroom))));

                return Ok(result);
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}
