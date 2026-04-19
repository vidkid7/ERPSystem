using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class LedgerGroup : BaseEntity
{
    public int? ParentGroupId { get; set; }
    public LedgerGroup? ParentGroup { get; set; }

    public NatureOfGroup NatureOfGroup { get; set; }
    public string? TypeOfGroup { get; set; }
    public bool IsDebtor { get; set; }
    public bool ShowInLedgerMaster { get; set; }
    public NumberingMethod NumberingMethod { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int NumericalPartWidth { get; set; }
    public int StartNumber { get; set; }
    public bool InBuilt { get; set; }

    public ICollection<LedgerGroup> ChildGroups { get; set; } = new List<LedgerGroup>();
    public ICollection<Ledger> Ledgers { get; set; } = new List<Ledger>();
}
