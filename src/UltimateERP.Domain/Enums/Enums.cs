namespace UltimateERP.Domain.Enums;

public enum LedgerType
{
    General = 0,
    Debtor = 1,
    Creditor = 2,
    Bank = 3,
    Cash = 4
}

public enum NatureOfGroup
{
    Asset = 0,
    Liability = 1,
    Income = 2,
    Expense = 3
}

public enum CostingMethod
{
    FIFO = 0,
    LIFO = 1,
    WeightedAverage = 2,
    StandardCost = 3
}

public enum VoucherStatus
{
    Draft = 0,
    Pending = 1,
    Authorized = 2,
    Posted = 3,
    Cancelled = 4
}

public enum InvoiceStatus
{
    Draft = 0,
    Pending = 1,
    Posted = 2,
    Cancelled = 3
}

public enum PDCStatus
{
    Pending = 0,
    Cleared = 1,
    Bounced = 2,
    Cancelled = 3
}

public enum LCStatus
{
    Open = 0,
    PartiallyUtilized = 1,
    Utilized = 2,
    Expired = 3,
    Cancelled = 4
}

public enum ProductionOrderStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum IndentStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Fulfilled = 3
}

public enum StockDemandStatus
{
    Pending = 0,
    Issued = 1,
    PartiallyIssued = 2
}

public enum DispatchStatus
{
    Pending = 0,
    Dispatched = 1,
    Delivered = 2,
    Cancelled = 3
}

public enum GatePassType
{
    Inward = 0,
    Outward = 1
}

public enum SalesAllotmentStatus
{
    Pending = 0,
    PartiallyDelivered = 1,
    Delivered = 2,
    Cancelled = 3
}

public enum AttendanceStatus
{
    Present = 0,
    Absent = 1,
    Leave = 2,
    Holiday = 3
}

public enum LeaveStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public enum ExpenseClaimStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Paid = 3
}

public enum AssetStatus
{
    Available = 0,
    Issued = 1,
    Damaged = 2,
    Repair = 3,
    Disposed = 4
}

public enum BedStatus
{
    Available = 0,
    Occupied = 1,
    Maintenance = 2
}

public enum IPDStatus
{
    Admitted = 0,
    Discharged = 1
}

public enum TicketPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum ComplaintTicketStatus
{
    Open = 0,
    Assigned = 1,
    InProgress = 2,
    Resolved = 3,
    Closed = 4
}

public enum JobCardStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum SparePartStatus
{
    Pending = 0,
    Issued = 1,
    Returned = 2
}

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum LoanStatus
{
    Active = 0,
    Closed = 1,
    Defaulted = 2
}

public enum EMIStatus
{
    Pending = 0,
    Paid = 1,
    Overdue = 2
}

public enum FixedProductStatus
{
    InStock = 0,
    Sold = 1,
    Financed = 2
}

public enum SampleCollectionStatus
{
    Pending = 0,
    Collected = 1,
    InProcess = 2,
    Completed = 3
}

public enum LabReportStatus
{
    Pending = 0,
    Completed = 1,
    Validated = 2
}

public enum NumberingMethod
{
    Manual = 0,
    Automatic = 1
}

public enum UDFFieldType
{
    Text = 0,
    Number = 1,
    Date = 2,
    Dropdown = 3
}

public enum ScheduledJobType
{
    IRDSync = 0,
    SMSReminder = 1,
    EMIAlert = 2,
    BackupJob = 3,
    StockAlert = 4,
    PDCAlert = 5
}

public enum JobRunStatus
{
    Running = 0,
    Completed = 1,
    Failed = 2
}

public enum LoyaltyTransactionType
{
    Earn = 0,
    Redeem = 1
}

public enum StockJournalType
{
    Adjustment = 0,
    Damage = 1,
    Consumption = 2
}

public enum ServiceAppointmentStatus
{
    Scheduled = 0,
    Completed = 1,
    Cancelled = 2
}

public enum SupportTicketStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2,
    Closed = 3
}

public enum KYCStatus
{
    Pending = 0,
    Verified = 1,
    Rejected = 2
}

public enum PaymentStatus
{
    Initiated = 0,
    Completed = 1,
    Failed = 2,
    Cancelled = 3
}

public enum FiscalYearStatus
{
    Open = 0,
    Closed = 1,
    Locked = 2
}
