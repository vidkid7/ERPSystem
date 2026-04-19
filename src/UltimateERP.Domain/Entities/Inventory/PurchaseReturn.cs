using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class PurchaseReturn : BranchAwareEntity
{
    public string ReturnNumber { get; set; } = string.Empty;
    public DateTime ReturnDate { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public string? InvoiceReference { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public decimal TotalAmount { get; set; }
    public PurchaseDocumentStatus Status { get; set; }
    public string? Remarks { get; set; }

    public ICollection<PurchaseReturnDetail> Details { get; set; } = new List<PurchaseReturnDetail>();
}

public class PurchaseReturnDetail : BaseEntity
{
    public int PurchaseReturnId { get; set; }
    public PurchaseReturn PurchaseReturn { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal ReturnQuantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}
