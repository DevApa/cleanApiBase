using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace bg.crm.integration.infrastructure.security
{
    public class RequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
                if (!context.ApiDescription.CustomAttributes().Any(x => x is AllowAnonymousAttribute)
                    && !context.ApiDescription.CustomAttributes().Any(x => x is AuthorizeAttribute)
                    || descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>() == null)
                {
                    operation.Parameters ??= new List<OpenApiParameter>();

                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                    operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Scheme = "bearer",
                                BearerFormat = "JWT",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.Http,
                                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",

                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    }
                };
                }
        }
    }
}