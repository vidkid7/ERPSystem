using AutoMapper;
using UltimateERP.Application.Features.AppCMS.DTOs;
using UltimateERP.Domain.Entities.CMS;

namespace UltimateERP.Application.Features.AppCMS.Mappings;

public class CMSMappingProfile : Profile
{
    public CMSMappingProfile()
    {
        CreateMap<Slider, SliderDto>();
        CreateMap<Banner, BannerDto>();
        CreateMap<Notice, NoticeDto>();
    }
}
