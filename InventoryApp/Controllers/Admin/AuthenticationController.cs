using InventoryApp.Contracts.Parameters;
using InventoryApp.Contracts.Responses;
using InventoryApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace InventoryApp.Controllers.Admin
{
    [ApiExplorerSettings(GroupName ="v1-admin")]
    [Route("api/admin/identity")]
    public class AuthenticationController : Controller
    {
        private readonly UserAuthenticationService _authenticationService;
       
        public AuthenticationController(UserAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationParameter parameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input credentials!");
            }
                
            var token = await _authenticationService.Authenticate(parameter.Email, parameter.Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                return NotFound("No such admin profile");
            }

            return Ok(new UserAuthenticationResponse
            {
                Token = token
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var data = await _authenticationService
                    .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());

                return Ok(new UserGetIdentityResponse
                {
                    Id = data.Id,
                    Email = data.Email,
                    Role = data.Role.ToString()
                });
            }
            catch (ArgumentException e)
            {
                return Unauthorized("Admin not found");
            }

        }

    }
}
