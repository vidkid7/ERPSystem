using AutoMapper;
using UltimateERP.Application.Features.Task.DTOs;
using UltimateERP.Domain.Entities.TaskModule;

namespace UltimateERP.Application.Features.Task.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s =>
                s.AssignedTo != null ? s.AssignedTo.Name : null))
            .ForMember(d => d.AssignedByName, o => o.MapFrom(s =>
                s.AssignedBy != null ? s.AssignedBy.Name : null))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
