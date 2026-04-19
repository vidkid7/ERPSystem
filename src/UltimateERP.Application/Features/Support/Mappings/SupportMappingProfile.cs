using AutoMapper;
using UltimateERP.Application.Features.Support.DTOs;
using UltimateERP.Domain.Entities.Support;

namespace UltimateERP.Application.Features.Support.Mappings;

public class SupportMappingProfile : Profile
{
    public SupportMappingProfile()
    {
        CreateMap<SupportTicket, SupportTicketDto>()
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s =>
                s.AssignedTo != null ? s.AssignedTo.Name : null))
            .ForMember(d => d.CreatedByName, o => o.MapFrom(s =>
                s.CreatedByUser != null ? s.CreatedByUser.Name : null))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
