using AutoMapper;
using UltimateERP.Application.Features.Assets.DTOs;
using UltimateERP.Domain.Entities.Assets;

namespace UltimateERP.Application.Features.Assets.Mappings;

public class AssetMappingProfile : Profile
{
    public AssetMappingProfile()
    {
        CreateMap<AssetMaster, AssetDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s =>
                s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
