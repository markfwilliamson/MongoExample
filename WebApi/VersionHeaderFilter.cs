using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    public class VersionHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("1.0")
                    },
                    Default = new OpenApiString("1.0")

                }
            });
        }
    }
}