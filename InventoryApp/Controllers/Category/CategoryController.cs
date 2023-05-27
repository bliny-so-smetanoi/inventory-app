using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Category;
using InventoryApp.Contracts.Parameters.Classroom;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace InventoryApp.Controllers.Category
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/category/")]
    [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
    public class CategoryController : Controller
    {
        private readonly ICategoryProvider _categoryProvider;
        private readonly ItemProvider _itemProvider;
        public CategoryController(ICategoryProvider categoryProvider,
            ItemProvider itemProvider)
        {
            _categoryProvider = categoryProvider;
            _itemProvider = itemProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryParameter categoryParameter)
        {
            try
            {
                var newCategory = new Models.Category
                {
                    Name = categoryParameter.Name,
                    Description = categoryParameter.Description,
                    ImageUrl = categoryParameter.ImageUrl,
                };

                await _categoryProvider.Add(newCategory);

                return Ok(new { message = "Category was added successfully!" });
            } catch (Exception)
            {
                return BadRequest(new {message = "Already exists!"});
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _categoryProvider.GetById(id);

                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _categoryProvider.GetAll();

                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CreateCategoryParameter editParameter)
        {
            try
            {
                var checkCategory = await _categoryProvider.FirstOrDefault(x =>
                    x.Name.Equals(editParameter.Name));
                var editCategory = await _categoryProvider.GetById(id) ??
                    throw new Exception("No such category");

                if (checkCategory is not null)
                {
                    if (!checkCategory.Id.Equals(editCategory.Id))
                    {
                        return BadRequest(new { message = "Category with current name/number is existing!" });
                    }
                }

                editCategory.Name = editParameter.Name;
                editCategory.Description = editParameter.Description;
                editCategory.ImageUrl = editParameter.ImageUrl;

                await _categoryProvider.Edit(editCategory);

                return Ok(new { message = "Category has been edited successfully!" });
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var category = await _categoryProvider.GetById(id);

                if (category is null) return NotFound();


                await _categoryProvider.Remove(category);
                var forDelete = await _itemProvider.Get(x => x.CategoryId == id);
                if (forDelete is null) return Ok();

                await _itemProvider.RemoveRange(forDelete);

                return Ok(new { message = "Category has been deleted successfully" });

            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search/{param}")]
        public async Task<IActionResult> SearchByName(string param)
        {
            try
            {
                var result = await _categoryProvider.Get(x => Regex.IsMatch(x.Name, param));

                return Ok(result);
            } catch(Exception)
            {
                return BadRequest();
            }

        }
    }
}
