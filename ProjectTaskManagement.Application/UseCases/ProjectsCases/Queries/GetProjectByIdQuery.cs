using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using Ardalis.GuardClauses;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.ProjectsCases.Queries;

public record GetProjectByIdQuery(int Id) : IRequest<DataResponse<ProjectDto>>;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, DataResponse<ProjectDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetProjectByIdQueryHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var project = await _unitOfWork.ProjectsRepository.GetOneAsync(
            new Specifications<Project>(p => p.Id == request.Id && p.UserId == userId));

        if (project is null)
            throw new NotFoundException(nameof(Project), request.Id.ToString());

        return new DataResponse<ProjectDto>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseData = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            }
        };
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }
}
