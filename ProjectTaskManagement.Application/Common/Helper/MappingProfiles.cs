using ProjectTaskManagement.Application.UseCases.ProjectsCases.DTO;
using ProjectTaskManagement.Application.UseCases.TasksCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using AutoMapper;

namespace ProjectTaskManagement.Application.Common.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectTask, TaskItemDto>();
    }
}
