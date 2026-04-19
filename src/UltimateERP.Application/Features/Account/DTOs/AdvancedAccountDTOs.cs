using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Account.DTOs;

// ── PDC ───────────────────────────────────────────────────────────────

public class PDCDto
{
    public int Id { get; set; }
    public int VoucherId { get; set; }
    public string ChequeNumber { get; set; } = string.Empty;
    public DateTime ChequeDate { get; set; }
    public string? ChequeDateBS { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public PDCStatus Status { get; set; }
    public DateTime? ClearedDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreatePDCDto
{
    public int VoucherId { get; set; }
    public string ChequeNumber { get; set; } = string.Empty;
    public DateTime ChequeDate { get; set; }
    public string? ChequeDateBS { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
}

// ── ODC ───────────────────────────────────────────────────────────────

public class ODCDto
{
    public int Id { get; set; }
    public int VoucherId { get; set; }
    public string ChequeNumber { get; set; } = string.Empty;
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
}

public class CreateODCDto
{
    public int VoucherId { get; set; }
    public string ChequeNumber { get; set; } = string.Empty;
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
}

// ── Bank Guarantee ────────────────────────────────────────────────────

public class BankGuaranteeDto
{
    public int Id { get; set; }
    public string GuaranteeNumber { get; set; } = string.Empty;
    public decimal GuaranteeAmount { get; set; }
    public string? IssuingBank { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? Purpose { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBankGuaranteeDto
{
    public string GuaranteeNumber { get; set; } = string.Empty;
    public decimal GuaranteeAmount { get; set; }
    public string? IssuingBank { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int LedgerId { get; set; }
    public string? Purpose { get; set; }
    public string? Status { get; set; }
}

// ── Letter of Credit ──────────────────────────────────────────────────

public class LetterOfCreditDto
{
    public int Id { get; set; }
    public string LCNumber { get; set; } = string.Empty;
    public DateTime OpeningDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal AmountInFC { get; set; }
    public int? LCCurrencyId { get; set; }
    public int? BankId { get; set; }
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? ShipmentTerms { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
    public LCStatus Status { get; set; }
    public bool IsActive { get; set; }
}

public class CreateLetterOfCreditDto
{
    public string LCNumber { get; set; } = string.Empty;
    public DateTime OpeningDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal AmountInFC { get; set; }
    public int? LCCurrencyId { get; set; }
    public int? BankId { get; set; }
    public int? VendorId { get; set; }
    public string? ShipmentTerms { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
}

// ── Bank Reconciliation ──────────────────────────────────────────────

public class BankReconciliationDto
{
    public int Id { get; set; }
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public int? VoucherDetailId { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? BankDate { get; set; }
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledDate { get; set; }
}

public class ReconcileTransactionDto
{
    public int Id { get; set; }
    public DateTime BankDate { get; set; }
}
