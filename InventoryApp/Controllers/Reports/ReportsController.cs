using InventoryApp.Contracts.Attributes;
using InventoryApp.Contracts.Parameters.Reports;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;
using InventoryApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace InventoryApp.Controllers.Reports
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/reports/")]
    [AdminAuthorized(UserRole.SuperAdmin, UserRole.Admin, UserRole.Moderator)]
    public class ReportsController : ControllerBase
    {
        private readonly ReportGeneratorService _reportService;
        private readonly UserAuthenticationService _authService;
        public ReportsController(ReportGeneratorService reportService, UserAuthenticationService authService) {
            _reportService = reportService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] CreateReportParameter createReportParameter)
        {
            try
            {
                Console.WriteLine("ok");
                var data = await _authService.GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
                await _reportService.GenerateReport(createReportParameter.Classroom, data.Id.ToString());

                return Ok(new {message = "Report was generated"});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
