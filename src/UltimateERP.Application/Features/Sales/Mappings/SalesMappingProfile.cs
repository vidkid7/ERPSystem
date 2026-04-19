using AutoMapper;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Sales.Mappings;

public class SalesMappingProfile : Profile
{
    public SalesMappingProfile()
    {
        CreateMap<SalesInvoice, SalesInvoiceDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.GodownName, o => o.MapFrom(s => s.Godown != null ? s.Godown.Name : null))
            .ForMember(d => d.SubTotal, o => o.MapFrom(s => s.TotalAmount))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.IsPosted ? "Posted" : "Draft"));

        CreateMap<SalesInvoiceDetail, SalesInvoiceDetailDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null));
    }
}
