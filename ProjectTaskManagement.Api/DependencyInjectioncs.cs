using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Api.Extensions;
using ProjectTaskManagement.Api.Services;

namespace ProjectTaskManagement.Api;

public static class DependencyInjectioncs
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddProblemDetails();
        services.AddJwtAuthentication(configuration);

        return services;
    }
}
