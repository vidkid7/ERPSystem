using AutoMapper;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Mappings;

public class AdvancedAccountMappingProfile : Profile
{
    public AdvancedAccountMappingProfile()
    {
        // PDC
        CreateMap<PDC, PDCDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
        CreateMap<CreatePDCDto, PDC>();

        // ODC
        CreateMap<ODC, ODCDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
        CreateMap<CreateODCDto, ODC>();

        // Bank Guarantee
        CreateMap<BankGuarantee, BankGuaranteeDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
        CreateMap<CreateBankGuaranteeDto, BankGuarantee>();

        // Letter of Credit
        CreateMap<LetterOfCredit, LetterOfCreditDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null));
        CreateMap<CreateLetterOfCreditDto, LetterOfCredit>();

        // Bank Reconciliation
        CreateMap<BankReconciliation, BankReconciliationDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
    }
}
