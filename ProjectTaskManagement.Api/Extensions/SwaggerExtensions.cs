using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace ProjectTaskManagement.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddOpenApi("v1", options =>
        {
            options.ShouldInclude = _ => true;

            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Project & Task Management API",
                    Version = "v1",
                    Description = "Clean Architecture API with JWT authentication"
                };

                document.Components ??= new OpenApiComponents();
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Bearer token. Example: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] = securityScheme;

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                var endpointMetadata = context.Description.ActionDescriptor?.EndpointMetadata;
                var allowsAnonymous = endpointMetadata?.Any(m => m is IAllowAnonymous) == true;

                if (allowsAnonymous)
                {
                    operation.Security = [];
                    return Task.CompletedTask;
                }

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                ];

                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static WebApplication UseApiDocumentation(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            // Must point to OpenAPI document (Swashbuckle's /swagger/v1/swagger.json stays empty for Minimal APIs)
            options.SwaggerEndpoint("/openapi/v1.json", "Project Task Management API v1");
            options.DocumentTitle = "Project & Task Management API";
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
