using AutoMapper;
using UltimateERP.Application.Features.Dispatch.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Dispatch.Mappings;

public class DispatchMappingProfile : Profile
{
    public DispatchMappingProfile()
    {
        CreateMap<DispatchOrder, DispatchOrderDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<DispatchSection, DispatchSectionDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null));
    }
}
