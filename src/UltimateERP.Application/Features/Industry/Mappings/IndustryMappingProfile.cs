using AutoMapper;
using UltimateERP.Application.Features.Industry.DTOs;
using UltimateERP.Domain.Entities.IndustrySpecific;

namespace UltimateERP.Application.Features.Industry.Mappings;

public class IndustryMappingProfile : Profile
{
    public IndustryMappingProfile()
    {
        CreateMap<DairyPurchaseInvoice, DairyPurchaseInvoiceDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null));
        CreateMap<CreateDairyPurchaseDto, DairyPurchaseInvoice>();

        CreateMap<TeaPurchaseInvoice, TeaPurchaseInvoiceDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null));
        CreateMap<CreateTeaPurchaseDto, TeaPurchaseInvoice>();

        CreateMap<PetrolPumpTransaction, PetrolPumpTransactionDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null));
        CreateMap<CreatePetrolPumpTransactionDto, PetrolPumpTransaction>();
    }
}
