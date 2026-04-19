using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class ReceiptNote : BranchAwareEntity
{
    public string GRNNumber { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public int? PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public PurchaseDocumentStatus Status { get; set; }
    public string? Remarks { get; set; }

    public ICollection<ReceiptNoteDetail> Details { get; set; } = new List<ReceiptNoteDetail>();
}

public class ReceiptNoteDetail : BaseEntity
{
    public int ReceiptNoteId { get; set; }
    public ReceiptNote ReceiptNote { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal ReceivedQuantity { get; set; }
    public decimal AcceptedQuantity { get; set; }
    public decimal RejectedQuantity { get; set; }
    public decimal Rate { get; set; }
}
