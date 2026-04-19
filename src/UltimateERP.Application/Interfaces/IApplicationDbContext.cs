using Microsoft.EntityFrameworkCore;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Assets;
using UltimateERP.Domain.Entities.CMS;
using UltimateERP.Domain.Entities.Finance;
using UltimateERP.Domain.Entities.HMS;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Entities.KYC;
using UltimateERP.Domain.Entities.Lab;
using UltimateERP.Domain.Entities.Loyalty;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Domain.Entities.Setup;
using UltimateERP.Domain.Entities.Support;
using UltimateERP.Domain.Entities.IndustrySpecific;
using UltimateERP.Domain.Entities.Reporting;
using UltimateERP.Domain.Entities.TaskModule;

namespace UltimateERP.Application.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext so Application layer doesn't depend on Infrastructure.
/// Add DbSet properties as needed by CQRS handlers.
/// </summary>
public interface IApplicationDbContext
{
    // Security
    DbSet<User> Users { get; }
    DbSet<UserGroup> UserGroups { get; }
    DbSet<UserGroupMember> UserGroupMembers { get; }
    DbSet<EntityPermission> EntityPermissions { get; }
    DbSet<ModuleAccess> ModuleAccesses { get; }
    DbSet<BranchAccess> BranchAccesses { get; }
    DbSet<GodownAccess> GodownAccesses { get; }
    DbSet<AuditLog> AuditLogs { get; }

    // Setup
    DbSet<Branch> Branches { get; }
    DbSet<CostClass> CostClasses { get; }
    DbSet<DocumentType> DocumentTypes { get; }
    DbSet<EntityNumbering> EntityNumberings { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<ExchangeRate> ExchangeRates { get; }

    // Account
    DbSet<Ledger> Ledgers { get; }
    DbSet<LedgerGroup> LedgerGroups { get; }
    DbSet<Voucher> Vouchers { get; }
    DbSet<VoucherDetail> VoucherDetails { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vendor> Vendors { get; }
    DbSet<PDC> PDCs { get; }
    DbSet<ODC> ODCs { get; }
    DbSet<BankGuarantee> BankGuarantees { get; }
    DbSet<LetterOfCredit> LettersOfCredit { get; }
    DbSet<BankReconciliation> BankReconciliations { get; }
    DbSet<PaymentTerm> PaymentTerms { get; }
    DbSet<PaymentMode> PaymentModes { get; }

    // Inventory
    DbSet<Product> Products { get; }
    DbSet<ProductGroup> ProductGroups { get; }
    DbSet<Godown> Godowns { get; }
    DbSet<Rack> Racks { get; }
    DbSet<Stock> Stocks { get; }
    DbSet<PurchaseInvoice> PurchaseInvoices { get; }
    DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; }
    DbSet<PurchaseQuotation> PurchaseQuotations { get; }
    DbSet<PurchaseQuotationDetail> PurchaseQuotationDetails { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; }
    DbSet<ReceiptNote> ReceiptNotes { get; }
    DbSet<ReceiptNoteDetail> ReceiptNoteDetails { get; }
    DbSet<PurchaseReturn> PurchaseReturns { get; }
    DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; }
    DbSet<PurchaseDebitNote> PurchaseDebitNotes { get; }
    DbSet<PurchaseCreditNote> PurchaseCreditNotes { get; }
    DbSet<SalesInvoice> SalesInvoices { get; }
    DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; }
    DbSet<SalesQuotation> SalesQuotations { get; }
    DbSet<SalesQuotationDetail> SalesQuotationDetails { get; }
    DbSet<SalesOrder> SalesOrders { get; }
    DbSet<SalesOrderDetail> SalesOrderDetails { get; }
    DbSet<SalesAllotment> SalesAllotments { get; }
    DbSet<SalesReturn> SalesReturns { get; }
    DbSet<SalesReturnDetail> SalesReturnDetails { get; }
    DbSet<SalesDebitNote> SalesDebitNotes { get; }
    DbSet<SalesCreditNote> SalesCreditNotes { get; }
    DbSet<Indent> Indents { get; }
    DbSet<IndentDetail> IndentDetails { get; }
    DbSet<GatePass> GatePasses { get; }
    DbSet<StockDemand> StockDemands { get; }
    DbSet<StockJournal> StockJournals { get; }
    DbSet<StockTransfer> StockTransfers { get; }
    DbSet<LandedCost> LandedCosts { get; }

    // HR
    DbSet<Employee> Employees { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<Leave> Leaves { get; }
    DbSet<ExpenseClaim> ExpenseClaims { get; }

    // HMS
    DbSet<Patient> Patients { get; }
    DbSet<Bed> Beds { get; }
    DbSet<OPDTicket> OPDTickets { get; }
    DbSet<IPDAdmission> IPDAdmissions { get; }

    // Service
    DbSet<Domain.Entities.Service.ComplaintTicket> ComplaintTickets { get; }
    DbSet<Domain.Entities.Service.JobCard> JobCards { get; }
    DbSet<Domain.Entities.Service.ServiceAppointment> ServiceAppointments { get; }

    // Task
    DbSet<TaskItem> TaskItems { get; }
    DbSet<TaskComment> TaskComments { get; }

    // Finance
    DbSet<Loan> Loans { get; }
    DbSet<LoanEMI> LoanEMIs { get; }

    // CMS
    DbSet<Slider> Sliders { get; }
    DbSet<Banner> Banners { get; }
    DbSet<Notice> Notices { get; }

    // Support
    DbSet<SupportTicket> SupportTickets { get; }

    // Loyalty
    DbSet<MembershipPoint> MembershipPoints { get; }

    // Manufacturing
    DbSet<BOM> BOMs { get; }
    DbSet<BOMDetail> BOMDetails { get; }
    DbSet<ProductionOrder> ProductionOrders { get; }

    // Lab
    DbSet<SampleCollection> SampleCollections { get; }
    DbSet<LabReport> LabReports { get; }

    // Assets
    DbSet<AssetMaster> AssetMasters { get; }
    DbSet<AssetGroup> AssetGroups { get; }
    DbSet<AssetType> AssetTypes { get; }
    DbSet<AssetModel> AssetModels { get; }
    DbSet<Asset> Assets { get; }
    DbSet<AssetCategory> AssetCategories { get; }
    DbSet<AssetTransaction> AssetTransactions { get; }

    // Dispatch
    DbSet<DispatchOrder> DispatchOrders { get; }

    // Industry-Specific
    DbSet<DairyPurchaseInvoice> DairyPurchaseInvoices { get; }
    DbSet<DairySalesInvoice> DairySalesInvoices { get; }
    DbSet<TeaPurchaseInvoice> TeaPurchaseInvoices { get; }
    DbSet<PetrolPumpTransaction> PetrolPumpTransactions { get; }

    // KYC
    DbSet<KYCRecord> KYCRecords { get; }

    // Reporting
    DbSet<ReportWriterDefinition> ReportWriterDefinitions { get; }

    // Account - Fiscal Year
    DbSet<FiscalYear> FiscalYears { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
