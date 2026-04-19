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

        // Rack
        CreateMap<Rack, RackDto>()
            .ForMember(d => d.GodownName, o => o.MapFrom(s => s.Godown != null ? s.Godown.Name : null));

        // Indent
        CreateMap<Indent, IndentDto>()
            .ForMember(d => d.IndentNo, o => o.MapFrom(s => s.IndentNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.IndentDate))
            .ForMember(d => d.RequestedBy, o => o.MapFrom(s => s.RequestedByEmployee != null ? s.RequestedByEmployee.Name : null))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<IndentDetail, IndentItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.RequestedQty, o => o.MapFrom(s => s.RequestedQuantity))
            .ForMember(d => d.ApprovedQty, o => o.MapFrom(s => s.ApprovedQuantity));

        // GatePass
        CreateMap<GatePass, GatePassDto>()
            .ForMember(d => d.GatePassNo, o => o.MapFrom(s => s.GatePassNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.GatePassDate))
            .ForMember(d => d.Type, o => o.MapFrom(s => s.GatePassType.ToString()))
            .ForMember(d => d.VehicleNo, o => o.MapFrom(s => s.VehicleNumber))
            .ForMember(d => d.PersonName, o => o.MapFrom(s => s.PartyName));

        // StockDemand
        CreateMap<StockDemand, StockDemandDto>()
            .ForMember(d => d.DemandNo, o => o.MapFrom(s => s.DemandNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.DemandDate))
            .ForMember(d => d.GodownName, o => o.MapFrom(s => s.Godown != null ? s.Godown.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // LandedCost
        CreateMap<LandedCost, LandedCostDto>()
            .ForMember(d => d.PurchaseInvoiceNumber, o => o.MapFrom(s => s.PurchaseInvoice != null ? s.PurchaseInvoice.InvoiceNumber : null))
            .ForMember(d => d.AllocationMethod, o => o.MapFrom(s => s.AllocationType.ToString()));
    }
}
