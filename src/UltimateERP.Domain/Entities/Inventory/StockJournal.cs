using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class StockJournal : BranchAwareEntity
{
    public string JournalNumber { get; set; } = string.Empty;
    public DateTime JournalDate { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public StockJournalType JournalType { get; set; }
    public string? Narration { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
}
