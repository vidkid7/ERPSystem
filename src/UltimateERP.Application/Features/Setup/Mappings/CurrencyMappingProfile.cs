using AutoMapper;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Application.Features.Setup.Mappings;

public class CurrencyMappingProfile : Profile
{
    public CurrencyMappingProfile()
    {
        CreateMap<Currency, CurrencyDto>();
        CreateMap<CreateCurrencyDto, Currency>();

        CreateMap<ExchangeRate, ExchangeRateDto>()
            .ForMember(d => d.FromCurrencyCode, o => o.MapFrom(s => s.FromCurrency != null ? s.FromCurrency.CurrencyCode : null))
            .ForMember(d => d.ToCurrencyCode, o => o.MapFrom(s => s.ToCurrency != null ? s.ToCurrency.CurrencyCode : null));
        CreateMap<CreateExchangeRateDto, ExchangeRate>();
    }
}
