namespace UltimateERP.Application.Features.Account.DTOs;

// ── Receipt Voucher ──────────────────────────────────────────────────

public class ReceiptVoucherDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public decimal Amount { get; set; }
    public int BankLedgerId { get; set; }
    public string? BankLedgerName { get; set; }
    public string? PaymentMode { get; set; }
    public string? ChequeNo { get; set; }
    public string? Narration { get; set; }
    public int? PartyLedgerId { get; set; }
    public string? PartyLedgerName { get; set; }
}

public class CreateReceiptVoucherDto
{
    public DateTime Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public decimal Amount { get; set; }
    public int BankLedgerId { get; set; }
    public int PartyLedgerId { get; set; }
    public string? PaymentMode { get; set; }
    public string? ChequeNo { get; set; }
    public string? Narration { get; set; }
}

// ── Payment Voucher ──────────────────────────────────────────────────

public class PaymentVoucherDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? PaidTo { get; set; }
    public decimal Amount { get; set; }
    public int BankLedgerId { get; set; }
    public string? BankLedgerName { get; set; }
    public string? PaymentMode { get; set; }
    public string? ChequeNo { get; set; }
    public string? Narration { get; set; }
    public int? PartyLedgerId { get; set; }
    public string? PartyLedgerName { get; set; }
}

public class CreatePaymentVoucherDto
{
    public DateTime Date { get; set; }
    public string? PaidTo { get; set; }
    public decimal Amount { get; set; }
    public int BankLedgerId { get; set; }
    public int PartyLedgerId { get; set; }
    public string? PaymentMode { get; set; }
    public string? ChequeNo { get; set; }
    public string? Narration { get; set; }
}

// ── Journal Voucher ──────────────────────────────────────────────────

public class JournalVoucherDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Narration { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public List<JournalLineDto> Lines { get; set; } = new();
}

public class JournalLineDto
{
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public int? CostCenterId { get; set; }
}

public class CreateJournalVoucherDto
{
    public DateTime Date { get; set; }
    public string? Narration { get; set; }
    public List<CreateJournalLineDto> Lines { get; set; } = new();
}

public class CreateJournalLineDto
{
    public int LedgerId { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public int? CostCenterId { get; set; }
}

// ── Contra Voucher ───────────────────────────────────────────────────

public class ContraVoucherDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int FromBankLedgerId { get; set; }
    public string? FromBankLedgerName { get; set; }
    public int ToBankLedgerId { get; set; }
    public string? ToBankLedgerName { get; set; }
    public decimal Amount { get; set; }
    public string? Narration { get; set; }
}

public class CreateContraVoucherDto
{
    public DateTime Date { get; set; }
    public int FromBankLedgerId { get; set; }
    public int ToBankLedgerId { get; set; }
    public decimal Amount { get; set; }
    public string? Narration { get; set; }
}

// ── Debit Note ───────────────────────────────────────────────────────

public class DebitNoteDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PartyLedgerId { get; set; }
    public string? PartyLedgerName { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? ReferenceInvoiceNo { get; set; }
}

public class CreateDebitNoteDto
{
    public DateTime Date { get; set; }
    public int PartyLedgerId { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? ReferenceInvoiceNo { get; set; }
}

// ── Credit Note ──────────────────────────────────────────────────────

public class CreditNoteDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PartyLedgerId { get; set; }
    public string? PartyLedgerName { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? ReferenceInvoiceNo { get; set; }
}

public class CreateCreditNoteDto
{
    public DateTime Date { get; set; }
    public int PartyLedgerId { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? ReferenceInvoiceNo { get; set; }
}
