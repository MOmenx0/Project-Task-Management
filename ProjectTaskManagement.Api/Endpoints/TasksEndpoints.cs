using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.UseCases.TasksCases.Commands;
using ProjectTaskManagement.Application.UseCases.TasksCases.DTO;
using ProjectTaskManagement.Application.UseCases.TasksCases.Queries;
using ProjectTaskManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTaskManagement.Api.Endpoints;

public static class TasksEndpoints
{
    public static RouteGroupBuilder MapTasksEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks")
            .WithTags("tasks")
            .WithGroupName("tasks")
            .RequireAuthorization();

        group.MapGet("/project/{projectId:int}", GetTasksByProject)
            .WithName("GetTasksByProject")
            .WithOpenApi();

        group.MapPost("/", CreateTask)
            .WithName("CreateTask")
            .WithOpenApi();

        group.MapPatch("/{taskId:int}/status", UpdateTaskStatus)
            .WithName("UpdateTaskStatus")
            .WithOpenApi();

        group.MapDelete("/{taskId:int}", DeleteTask)
            .WithName("DeleteTask")
            .WithOpenApi();

        return group;
    }

    private static async Task<DataResponse<IEnumerable<TaskItemDto>>> GetTasksByProject(
        [FromServices] ISender sender,
        int projectId) =>
        await sender.Send(new GetTasksByProjectQuery(projectId));

    private static async Task<DataResponse<TaskItemDto>> CreateTask(
        [FromServices] ISender sender,
        [FromBody] CreateTaskCommand command) =>
        await sender.Send(command);

    private static async Task<DataResponse<TaskItemDto>> UpdateTaskStatus(
        [FromServices] ISender sender,
        int taskId,
        [FromBody] UpdateTaskStatusRequest request) =>
        await sender.Send(new UpdateTaskStatusCommand(taskId, request.Status));

    private static async Task<DataResponse<bool>> DeleteTask([FromServices] ISender sender, int taskId) =>
        await sender.Send(new DeleteTaskCommand(taskId));
}

public record UpdateTaskStatusRequest(TaskItemStatus Status);
