using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Loyalty;

public class MembershipPoint : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public DateTime TransactionDate { get; set; }
    public LoyaltyTransactionType TransactionType { get; set; }
    public decimal Points { get; set; }
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
    public decimal Balance { get; set; }
}
