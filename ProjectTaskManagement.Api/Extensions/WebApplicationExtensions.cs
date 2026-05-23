using ProjectTaskManagement.Api.Endpoints;

namespace ProjectTaskManagement.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapProjectsEndpoints();
        app.MapTasksEndpoints();
        return app;
    }
}
