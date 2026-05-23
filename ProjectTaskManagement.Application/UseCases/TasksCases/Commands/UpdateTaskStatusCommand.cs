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

public record UpdateTaskStatusCommand(int TaskId, TaskItemStatus Status) : IRequest<DataResponse<TaskItemDto>>;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, DataResponse<TaskItemDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateTaskStatusCommandHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<TaskItemDto>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var task = await GetOwnedTaskAsync(request.TaskId, userId);

        if (task is null)
            throw new NotFoundException(nameof(ProjectTask), request.TaskId.ToString());

        task.Status = request.Status;
        _unitOfWork.TasksRepository.Update(task);
        await _unitOfWork.SaveChangesAsync();

        return new DataResponse<TaskItemDto>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseData = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                Priority = task.Priority,
                ProjectId = task.ProjectId
            }
        };
    }

    private async Task<ProjectTask?> GetOwnedTaskAsync(int taskId, int userId)
    {
        var task = await _unitOfWork.TasksRepository.GetOneAsync(
            new Specifications<ProjectTask>(t => t.Id == taskId));

        if (task is null)
            return null;

        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == task.ProjectId && p.UserId == userId));

        return project is null ? null : task;
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }
}
