using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.TasksCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using ProjectTaskManagement.Domain.Enums;
using Ardalis.GuardClauses;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.TasksCases.Commands;

public record CreateTaskCommand(
    int ProjectId,
    string Title,
    string Description,
    DateTime? DueDate,
    TaskPriority Priority) : IRequest<DataResponse<TaskItemDto>>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, DataResponse<TaskItemDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateTaskCommandHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<TaskItemDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        await EnsureProjectOwnership(request.ProjectId, userId);

        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Status = TaskItemStatus.Todo
        };

        await _unitOfWork.TasksRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return new DataResponse<TaskItemDto>
        {
            StatusCode = HttpStatusCode.Created,
            ResponseData = MapToDto(task)
        };
    }

    private async Task EnsureProjectOwnership(int projectId, int userId)
    {
        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == projectId && p.UserId == userId));

        if (project is null)
            throw new NotFoundException(nameof(Project), projectId.ToString());
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }

    private static TaskItemDto MapToDto(ProjectTask task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        DueDate = task.DueDate,
        Priority = task.Priority,
        ProjectId = task.ProjectId
    };
}
