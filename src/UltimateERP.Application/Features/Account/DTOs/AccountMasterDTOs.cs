namespace UltimateERP.Application.Features.Account.DTOs;

// ── PaymentTerm ──────────────────────────────────────────────────────

public class PaymentTermDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueDays { get; set; }
    public decimal DiscountPercent { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreatePaymentTermDto
{
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueDays { get; set; }
    public decimal DiscountPercent { get; set; }
    public string? Description { get; set; }
}

public class UpdatePaymentTermDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueDays { get; set; }
    public decimal DiscountPercent { get; set; }
    public string? Description { get; set; }
}

// ── PaymentMode ──────────────────────────────────────────────────────

public class PaymentModeDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreatePaymentModeDto
{
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpdatePaymentModeDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
