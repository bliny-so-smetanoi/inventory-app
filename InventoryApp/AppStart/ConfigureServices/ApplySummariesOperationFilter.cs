using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            {
                return;
            }

            var actionName = controllerActionDescriptor.ActionName;
            if (actionName != "GetPaged")
            {
                return;
            }

            var resourceName = controllerActionDescriptor.ControllerName;
            operation.Summary = $"Returns paged list of the {resourceName} as IPagedList wrapped with OperationResult";
        }

    }
}
