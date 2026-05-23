using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.Commands;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.DTO;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTaskManagement.Api.Endpoints;

public static class ProjectsEndpoints
{
    public static RouteGroupBuilder MapProjectsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("projects")
            .WithGroupName("projects")
            .RequireAuthorization();

        group.MapGet("/", GetAllProjects)
            .WithName("GetAllProjects")
            .WithOpenApi();

        group.MapGet("/{id:int}", GetProjectById)
            .WithName("GetProjectById")
            .WithOpenApi();

        group.MapPost("/", CreateProject)
            .WithName("CreateProject")
            .WithOpenApi();

        group.MapPut("/{id:int}", UpdateProject)
            .WithName("UpdateProject")
            .WithOpenApi();

        group.MapDelete("/{id:int}", DeleteProject)
            .WithName("DeleteProject")
            .WithOpenApi();

        return group;
    }

    private static async Task<DataResponse<IEnumerable<ProjectDto>>> GetAllProjects([FromServices] ISender sender) =>
        await sender.Send(new GetAllProjectsQuery());

    private static async Task<DataResponse<ProjectDto>> GetProjectById([FromServices] ISender sender, int id) =>
        await sender.Send(new GetProjectByIdQuery(id));

    private static async Task<DataResponse<ProjectDto>> CreateProject(
        [FromServices] ISender sender,
        [FromBody] CreateProjectCommand command) =>
        await sender.Send(command);

    private static async Task<DataResponse<ProjectDto>> UpdateProject(
        [FromServices] ISender sender,
        int id,
        [FromBody] UpdateProjectRequest request) =>
        await sender.Send(new UpdateProjectCommand(id, request.Name, request.Description));

    private static async Task<DataResponse<bool>> DeleteProject([FromServices] ISender sender, int id) =>
        await sender.Send(new DeleteProjectCommand(id));
}

public record UpdateProjectRequest(string Name, string Description);
