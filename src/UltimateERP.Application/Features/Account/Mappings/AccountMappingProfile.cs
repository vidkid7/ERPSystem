using AutoMapper;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<LedgerGroup, LedgerGroupDto>()
            .ForMember(d => d.ParentGroupName, o => o.MapFrom(s => s.ParentGroup != null ? s.ParentGroup.Name : null));

        CreateMap<CreateLedgerGroupDto, LedgerGroup>();

        CreateMap<Ledger, LedgerDto>()
            .ForMember(d => d.LedgerGroupName, o => o.MapFrom(s => s.LedgerGroup != null ? s.LedgerGroup.Name : null));

        CreateMap<CreateLedgerDto, Ledger>();

        CreateMap<Voucher, VoucherDto>();
        CreateMap<VoucherDetail, VoucherDetailDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));

        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
        CreateMap<CreateCustomerDto, Customer>();

        CreateMap<Vendor, VendorDto>()
            .ForMember(d => d.LedgerName, o => o.MapFrom(s => s.Ledger != null ? s.Ledger.Name : null));
        CreateMap<CreateVendorDto, Vendor>();
    }
}
