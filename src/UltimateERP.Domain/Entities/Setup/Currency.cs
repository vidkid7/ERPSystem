using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Setup;

public class Currency : BaseEntity
{
    public string CurrencyCode { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public int DecimalPlaces { get; set; } = 2;
    public bool IsBaseCurrency { get; set; }
}
