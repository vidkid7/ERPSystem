using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesDebitNote : BranchAwareEntity
{
    public string NoteNumber { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public string? NoteDateBS { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Reason { get; set; }
    public DebitCreditNoteStatus Status { get; set; }
}
