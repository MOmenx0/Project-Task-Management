using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.ProjectsCases.Queries;

public record GetAllProjectsQuery : IRequest<DataResponse<IEnumerable<ProjectDto>>>;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, DataResponse<IEnumerable<ProjectDto>>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetAllProjectsQueryHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var projects = await _unitOfWork.ProjectsRepository.GetListAsync(
            new Specifications<Project>(p => p.UserId == userId));

        var dtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        });

        return new DataResponse<IEnumerable<ProjectDto>>
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
