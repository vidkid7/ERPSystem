using AutoMapper;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Mappings;

public class AccountMasterMappingProfile : Profile
{
    public AccountMasterMappingProfile()
    {
        CreateMap<PaymentTerm, PaymentTermDto>();
        CreateMap<CreatePaymentTermDto, PaymentTerm>();

        CreateMap<PaymentMode, PaymentModeDto>();
        CreateMap<CreatePaymentModeDto, PaymentMode>();
    }
}
