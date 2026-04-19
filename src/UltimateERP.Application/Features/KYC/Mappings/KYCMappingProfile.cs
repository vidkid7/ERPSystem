using AutoMapper;
using UltimateERP.Application.Features.KYC.DTOs;
using UltimateERP.Domain.Entities.KYC;

namespace UltimateERP.Application.Features.KYC.Mappings;

public class KYCMappingProfile : Profile
{
    public KYCMappingProfile()
    {
        CreateMap<KYCRecord, KYCRecordDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null));

        CreateMap<CreateKYCDto, KYCRecord>();

        CreateMap<UpdateKYCDto, KYCRecord>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
