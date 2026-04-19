using AutoMapper;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Purchase.Mappings;

public class PurchaseMappingProfile : Profile
{
    public PurchaseMappingProfile()
    {
        CreateMap<PurchaseInvoice, PurchaseInvoiceDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.GodownName, o => o.MapFrom(s => s.Godown != null ? s.Godown.Name : null))
            .ForMember(d => d.SubTotal, o => o.MapFrom(s => s.TotalAmount))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.IsPosted ? "Posted" : "Draft"));

        CreateMap<PurchaseInvoiceDetail, PurchaseInvoiceDetailDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null));
    }
}
