using AutoMapper;
using UltimateERP.Application.Features.Manufacturing.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Manufacturing.Mappings;

public class ManufacturingMappingProfile : Profile
{
    public ManufacturingMappingProfile()
    {
        CreateMap<BOM, BOMDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s =>
                s.Product != null ? s.Product.Name : null));

        CreateMap<BOMDetail, BOMDetailDto>()
            .ForMember(d => d.ComponentProductName, o => o.MapFrom(s =>
                s.ComponentProduct != null ? s.ComponentProduct.Name : null));

        CreateMap<ProductionOrder, ProductionOrderDto>()
            .ForMember(d => d.FinishedProductName, o => o.MapFrom(s =>
                s.FinishedProduct != null ? s.FinishedProduct.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
