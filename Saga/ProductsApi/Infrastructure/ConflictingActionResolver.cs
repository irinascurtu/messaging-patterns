using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductsApi.Infrastructure
{
    public class ConflictingActionsResolver : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            var actionDescriptor = apiDescription.ActionDescriptor;

            // Customize the operation id to be unique
            operation.OperationId = $"{actionDescriptor.RouteValues["controller"]}_{actionDescriptor.RouteValues["action"]}";
        }
    }

}
