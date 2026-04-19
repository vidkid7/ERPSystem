using AutoMapper;
using UltimateERP.Application.Features.Lab.DTOs;
using UltimateERP.Domain.Entities.Lab;

namespace UltimateERP.Application.Features.Lab.Mappings;

public class LabMappingProfile : Profile
{
    public LabMappingProfile()
    {
        CreateMap<SampleCollection, SampleCollectionDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<LabReport, LabReportDto>()
            .ForMember(d => d.SampleNumber, o => o.MapFrom(s =>
                s.SampleCollection != null ? s.SampleCollection.SampleNumber : null))
            .ForMember(d => d.PatientName, o => o.MapFrom(s =>
                s.SampleCollection != null ? s.SampleCollection.PatientName : null));
    }
}
