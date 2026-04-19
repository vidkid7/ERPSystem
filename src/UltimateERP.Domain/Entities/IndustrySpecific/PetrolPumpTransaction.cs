using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class PetrolPumpTransaction : BranchAwareEntity
{
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? TransactionDateBS { get; set; }
    public int? NozzleId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal OpeningReading { get; set; }
    public decimal ClosingReading { get; set; }
    public decimal QuantityDispensed { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public bool IsPosted { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
}
