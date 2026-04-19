namespace UltimateERP.Application.Features.Setup.DTOs;

public class BranchDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? PAN { get; set; }
    public bool IsActive { get; set; }
}

public class CostClassDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class DocumentTypeDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public string? Module { get; set; }
    public bool IsActive { get; set; }
}

public class EntityNumberingDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int NumericalPartWidth { get; set; }
    public int StartNumber { get; set; }
    public int CurrentNumber { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBranchDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? PAN { get; set; }
}

public class UpdateBranchDto : CreateBranchDto
{
    public int Id { get; set; }
}

public class CreateCostClassDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateDocumentTypeDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public string? Module { get; set; }
}

public class CreateEntityNumberingDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int NumericalPartWidth { get; set; } = 4;
    public int StartNumber { get; set; } = 1;
}
