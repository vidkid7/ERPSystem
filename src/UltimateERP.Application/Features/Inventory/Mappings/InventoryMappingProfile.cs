using AutoMapper;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Inventory.Mappings;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.ProductGroupName, o => o.MapFrom(s => s.ProductGroup != null ? s.ProductGroup.Name : null));
        CreateMap<CreateProductDto, Product>();

        CreateMap<ProductGroup, ProductGroupDto>()
            .ForMember(d => d.ParentGroupName, o => o.MapFrom(s => s.ParentGroup != null ? s.ParentGroup.Name : null));
        CreateMap<CreateProductGroupDto, ProductGroup>();

        CreateMap<Godown, GodownDto>();
        CreateMap<CreateGodownDto, Godown>();

        CreateMap<Stock, StockDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.ProductCode, o => o.MapFrom(s => s.Product != null ? s.Product.Code : null))
            .ForMember(d => d.GodownName, o => o.MapFrom(s => s.Godown != null ? s.Godown.Name : null))
            .ForMember(d => d.Value, o => o.MapFrom(s => s.Quantity * s.Rate));
    }
}
