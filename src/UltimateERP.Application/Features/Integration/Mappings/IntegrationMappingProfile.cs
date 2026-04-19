using AutoMapper;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Application.Features.Integration.Mappings;

public class IntegrationMappingProfile : Profile
{
    public IntegrationMappingProfile()
    {
        CreateMap<SmsLog, SmsLogDto>();
    }
}
