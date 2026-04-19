namespace UltimateERP.Application.Features.Setup.DTOs;

public class MergeLedgersDto
{
    public int SourceLedgerId { get; set; }
    public int TargetLedgerId { get; set; }
}

public class MergeProductsDto
{
    public int SourceProductId { get; set; }
    public int TargetProductId { get; set; }
}

public class RenumberVouchersDto
{
    public int DocumentTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StartNumber { get; set; } = 1;
}

public class MergeResultDto
{
    public bool IsSuccess { get; set; }
    public int RecordsUpdated { get; set; }
    public string? Message { get; set; }
}
