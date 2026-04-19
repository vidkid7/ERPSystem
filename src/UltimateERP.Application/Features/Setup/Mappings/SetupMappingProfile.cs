using AutoMapper;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Application.Features.Setup.Mappings;

public class SetupMappingProfile : Profile
{
    public SetupMappingProfile()
    {
        CreateMap<Branch, BranchDto>();
        CreateMap<CreateBranchDto, Branch>();

        CreateMap<CostClass, CostClassDto>();
        CreateMap<CreateCostClassDto, CostClass>();

        CreateMap<DocumentType, DocumentTypeDto>();
        CreateMap<CreateDocumentTypeDto, DocumentType>();

        CreateMap<EntityNumbering, EntityNumberingDto>();
        CreateMap<CreateEntityNumberingDto, EntityNumbering>();
    }
}
