using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.Account;

public class Voucher : BranchAwareEntity
{
    public int VoucherTypeId { get; set; }
    public string VoucherNumber { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; }
    public string? VoucherDateBS { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public string? CommonNarration { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }

    public bool IsAuthorized { get; set; }
    public int? AuthorizedBy { get; set; }
    public DateTime? AuthorizedDate { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
    public bool IsCancelled { get; set; }
    public int? CancelledBy { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancelReason { get; set; }

    public ICollection<VoucherDetail> Details { get; set; } = new List<VoucherDetail>();
    public ICollection<PDC> PDCs { get; set; } = new List<PDC>();
    public ICollection<ODC> ODCs { get; set; } = new List<ODC>();
}
