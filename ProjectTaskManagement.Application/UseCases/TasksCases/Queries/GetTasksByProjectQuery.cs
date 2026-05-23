using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.TasksCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using Ardalis.GuardClauses;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.TasksCases.Queries;

public record GetTasksByProjectQuery(int ProjectId) : IRequest<DataResponse<IEnumerable<TaskItemDto>>>;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, DataResponse<IEnumerable<TaskItemDto>>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetTasksByProjectQueryHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<IEnumerable<TaskItemDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == request.ProjectId && p.UserId == userId));

        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

        var tasks = await _unitOfWork.TasksRepository.GetListAsync(
            new Specifications<ProjectTask>(t => t.ProjectId == request.ProjectId));

        var dtos = tasks.Select(t => new TaskItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            DueDate = t.DueDate,
            Priority = t.Priority,
            ProjectId = t.ProjectId
        });

        return new DataResponse<IEnumerable<TaskItemDto>>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseData = dtos
        };
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }
}
