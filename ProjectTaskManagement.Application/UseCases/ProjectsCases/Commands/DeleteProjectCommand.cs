using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Domain.Entities;
using Ardalis.GuardClauses;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.ProjectsCases.Commands;

public record DeleteProjectCommand(int Id) : IRequest<DataResponse<bool>>;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, DataResponse<bool>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteProjectCommandHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == request.Id && p.UserId == userId));

        if (project is null)
            throw new NotFoundException(nameof(Project), request.Id.ToString());

        var tasks = await _unitOfWork.TasksRepository.GetListAsync(
            new Specifications<ProjectTask>(t => t.ProjectId == project.Id));

        foreach (var task in tasks)
            _unitOfWork.TasksRepository.Delete(task);

        _unitOfWork.ProjectsRepository.Delete(project);
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
