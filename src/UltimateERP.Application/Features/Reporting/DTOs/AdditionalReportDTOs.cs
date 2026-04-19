namespace UltimateERP.Application.Features.Reporting.DTOs;

public class CancelDayBookDto
{
    public int VoucherId { get; set; }
    public string VoucherNumber { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; }
    public string? VoucherType { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancelReason { get; set; }
    public decimal Amount { get; set; }
}

public class CostCenterAnalysisDto
{
    public int CostCenterId { get; set; }
    public string CostCenterName { get; set; } = string.Empty;
    public decimal DebitTotal { get; set; }
    public decimal CreditTotal { get; set; }
    public decimal NetAmount { get; set; }
}

public class BillsReceivableDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal ReceivedAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public int AgingDays { get; set; }
}

public class BillsPayableDto
{
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public int AgingDays { get; set; }
}

public class StatisticVoucherDto
{
    public string VoucherType { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class LedgerCurrentStatusDto
{
    public int LedgerId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
    public string? GroupName { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string BalanceType { get; set; } = string.Empty;
}

public class PendingPOSummaryDto
{
    public int PurchaseOrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ReceivedAmount { get; set; }
    public decimal PendingAmount { get; set; }
}
