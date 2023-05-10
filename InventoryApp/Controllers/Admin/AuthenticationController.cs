using InventoryApp.Contracts.Parameters;
using InventoryApp.Contracts.Responses;
using InventoryApp.DataAccess.Providers.Interfaces;
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
        private readonly IReportProvider _reportProvider;

        public AuthenticationController(UserAuthenticationService authenticationService,
            IReportProvider reportProvider)
        {
            _authenticationService = authenticationService;
            _reportProvider = reportProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationParameter parameter)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input credentials!");
            }
                
            var (token, role) = await _authenticationService.Authenticate(parameter.Email, parameter.Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                return NotFound("No such admin profile");
            }

            return Ok(new UserAuthenticationResponse
            {
                Token = token,
                Role = role
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var data = await _authenticationService
                    .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
                var reports = await _reportProvider.Get(x => x.UserId.Equals(data.Id));

                return Ok(new UserGetIdentityResponse
                {
                    Id = data.Id,
                    Email = data.Email,
                    Role = data.Role.ToString(),
                    ReportsUrl = reports
                });
            }
            catch (ArgumentException e)
            {
                return Unauthorized("Admin not found");
            }

        }

    }
}
