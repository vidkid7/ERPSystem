using AutoMapper;
using UltimateERP.Application.Features.Finance.DTOs;
using UltimateERP.Domain.Entities.Finance;

namespace UltimateERP.Application.Features.Finance.Mappings;

public class FinanceMappingProfile : Profile
{
    public FinanceMappingProfile()
    {
        CreateMap<Loan, LoanDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<LoanEMI, LoanEMIDto>()
            .ForMember(d => d.LoanNumber, o => o.MapFrom(s => s.Loan != null ? s.Loan.LoanNumber : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
