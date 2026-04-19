using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class MeterReading : BaseEntity
{
    public DateTime ReadingDate { get; set; }
    public string? ReadingDateBS { get; set; }
    public int? NozzleId { get; set; }
    public decimal OpeningReading { get; set; }
    public decimal ClosingReading { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
}
