using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.UseCases.ProjectsCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.ProjectsCases.Commands;

public record CreateProjectCommand(string Name, string Description) : IRequest<DataResponse<ProjectDto>>;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, DataResponse<ProjectDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(IunitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<DataResponse<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _unitOfWork.ProjectsRepository.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return new DataResponse<ProjectDto>
        {
            StatusCode = HttpStatusCode.Created,
            ResponseData = MapToDto(project)
        };
    }

    private int GetCurrentUserId()
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return _currentUser.UserId.Value;
    }

    private static ProjectDto MapToDto(Project project) => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        CreatedAt = project.CreatedAt
    };
}
