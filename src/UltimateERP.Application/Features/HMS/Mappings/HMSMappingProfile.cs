using AutoMapper;
using UltimateERP.Application.Features.HMS.DTOs;
using UltimateERP.Domain.Entities.HMS;

namespace UltimateERP.Application.Features.HMS.Mappings;

public class HMSMappingProfile : Profile
{
    public HMSMappingProfile()
    {
        CreateMap<Patient, PatientDto>();

        CreateMap<Bed, BedDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<OPDTicket, OPDTicketDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s =>
                s.Patient != null ? $"{s.Patient.FirstName} {s.Patient.LastName}" : null));

        CreateMap<IPDAdmission, IPDAdmissionDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s =>
                s.Patient != null ? $"{s.Patient.FirstName} {s.Patient.LastName}" : null))
            .ForMember(d => d.BedNumber, o => o.MapFrom(s => s.Bed != null ? s.Bed.BedNumber : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
