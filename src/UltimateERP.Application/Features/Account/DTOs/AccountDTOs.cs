namespace UltimateERP.Application.Features.Account.DTOs;

// ── LedgerGroup ───────────────────────────────────────────────────────

public class LedgerGroupDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGroupId { get; set; }
    public string? ParentGroupName { get; set; }
    public bool IsActive { get; set; }
    public List<LedgerGroupDto> Children { get; set; } = new();
}

public class CreateLedgerGroupDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGroupId { get; set; }
}

// ── Ledger ────────────────────────────────────────────────────────────

public class LedgerDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LedgerGroupId { get; set; }
    public string? LedgerGroupName { get; set; }
    public string? LedgerType { get; set; }
    public decimal OpeningBalance { get; set; }
    public string? BalanceType { get; set; }
    public bool IsActive { get; set; }
}

public class CreateLedgerDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LedgerGroupId { get; set; }
    public string? LedgerType { get; set; }
    public decimal OpeningBalance { get; set; }
    public string? BalanceType { get; set; }
}

// ── Voucher ───────────────────────────────────────────────────────────

public class VoucherDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public string? VoucherType { get; set; }
    public string? Narration { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<VoucherDetailDto> Details { get; set; } = new();
}

public class VoucherDetailDto
{
    public int Id { get; set; }
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Narration { get; set; }
}

public class CreateVoucherDto
{
    public DateTime VoucherDate { get; set; }
    public string? VoucherType { get; set; }
    public string? Narration { get; set; }
    public List<CreateVoucherDetailDto> Details { get; set; } = new();
}

public class CreateVoucherDetailDto
{
    public int LedgerId { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Narration { get; set; }
}

// ── Customer / Vendor ─────────────────────────────────────────────────

public class CustomerDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PAN { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCustomerDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PAN { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
}

public class VendorDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PAN { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
    public bool IsActive { get; set; }
}

public class CreateVendorDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PAN { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
}
