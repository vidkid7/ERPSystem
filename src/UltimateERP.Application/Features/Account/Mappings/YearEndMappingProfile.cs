using AutoMapper;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Mappings;

public class YearEndMappingProfile : Profile
{
    public YearEndMappingProfile()
    {
        CreateMap<FiscalYear, FiscalYearDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
