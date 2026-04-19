using AutoMapper;
using UltimateERP.Application.Features.Service.DTOs;
using UltimateERP.Domain.Entities.Service;

namespace UltimateERP.Application.Features.Service.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<ComplaintTicket, ComplaintTicketDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<JobCard, JobCardDto>()
            .ForMember(d => d.ComplaintTicketNumber, o => o.MapFrom(s =>
                s.ComplaintTicket != null ? s.ComplaintTicket.TicketNumber : null))
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s =>
                s.AssignedTo != null ? $"{s.AssignedTo.FirstName} {s.AssignedTo.LastName}" : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<ServiceAppointment, ServiceAppointmentDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s =>
                s.AssignedTo != null ? $"{s.AssignedTo.FirstName} {s.AssignedTo.LastName}" : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
