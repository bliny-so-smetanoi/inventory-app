using Amazon.S3.Model;
using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Admin;
using InventoryApp.Contracts.Responses;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;
using InventoryApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace InventoryApp.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/admin/")]
    [AdminAuthorized(roles: UserRole.SuperAdmin)]
    public class AdminController : Controller
    {
        private readonly IUserProvider _userProvider;

        public AdminController(IUserProvider userProvider) {
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userProvider.GetAllAdminsAndModerators();

            return Ok(result); 
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var admin = await _userProvider.GetById(id);

            if(admin is null || admin.Role == UserRole.SuperAdmin) return NotFound();

            var result = new GetAdminsResponse
            {
                Id = admin.Id,
                Email = admin.Email,
                FullName= admin.FullName,
                Role = admin.Role,
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminParameter parameter)
        {
            try
            {
                Console.WriteLine(parameter.Role);
                var user = await _userProvider.FirstOrDefault(x => x.Email == parameter.Email);

                if (user is not null)
                    return BadRequest("Admin or moderator with current email existed");

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(parameter.Password);

                var newUser = new User
                {
                    Email = parameter.Email,
                    FullName = parameter.FullName,
                    Role = (UserRole) Int32.Parse(parameter.Role),
                    Password = passwordHash
                };

                await _userProvider.Add(newUser);

                return Ok(new {message = "User was added!"});
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] AdminParameter parameter)
        {
            try
            {
                var moderator = await _userProvider.FirstOrDefault(x => x.Email.Equals(parameter.Email));
                var admin = await _userProvider.GetById(id) ?? throw new ArgumentException("No such admin");

                if (moderator is not null)
                {
                    if (!moderator.Id.Equals(admin.Id))
                        return BadRequest("Admin with current login is existing!");
                }

                if (admin is null || admin.Role == UserRole.SuperAdmin) return NotFound();
                
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(parameter.Password);

                admin.Email = parameter.Email;
                admin.Role = (UserRole)Int32.Parse(parameter.Role);
                admin.FullName = parameter.FullName;
                admin.Password = passwordHash;

                await _userProvider.Edit(admin);

                return Ok(new {message = "User was edited!"});
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var admin = await _userProvider.GetById(id);

            if (admin is null || admin.Role == UserRole.SuperAdmin) return NotFound();

            await _userProvider.Remove(admin);

            return Ok(new {message = "User was deleted!"});
        }

        [HttpGet("search/{param}")]
        public async Task<IActionResult> SearchByEmail(string param)
        {
            try
            {
                
                var res = await _userProvider.Get(x => Regex.IsMatch(x.Email, param));

                return Ok(res);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
