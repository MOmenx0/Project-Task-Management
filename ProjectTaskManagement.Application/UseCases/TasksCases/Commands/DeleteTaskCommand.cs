using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Domain.Entities;
using Ardalis.GuardClauses;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.TasksCases.Commands;

public record DeleteTaskCommand(int TaskId) : IRequest<DataResponse<bool>>;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DataResponse<bool>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteTaskCommandHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var task = await _unitOfWork.TasksRepository.GetOneAsync(
            new Specifications<ProjectTask>(t => t.Id == request.TaskId));

        if (task is null)
            throw new NotFoundException(nameof(ProjectTask), request.TaskId.ToString());

        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == task.ProjectId && p.UserId == userId));

        if (project is null)
            throw new NotFoundException(nameof(ProjectTask), request.TaskId.ToString());

        _unitOfWork.TasksRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync();

        return new DataResponse<bool>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseData = true
        };
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }
}
