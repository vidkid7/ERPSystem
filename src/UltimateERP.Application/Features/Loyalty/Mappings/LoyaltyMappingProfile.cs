using AutoMapper;
using UltimateERP.Application.Features.Loyalty.DTOs;
using UltimateERP.Domain.Entities.Loyalty;

namespace UltimateERP.Application.Features.Loyalty.Mappings;

public class LoyaltyMappingProfile : Profile
{
    public LoyaltyMappingProfile()
    {
        CreateMap<MembershipPoint, MembershipPointDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s =>
                s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.TransactionType, o => o.MapFrom(s => s.TransactionType.ToString()));
    }
}
