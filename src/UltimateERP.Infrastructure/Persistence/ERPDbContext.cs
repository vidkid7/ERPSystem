using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UltimateERP.Domain.Entities.Setup;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Entities.Assets;
using UltimateERP.Domain.Entities.Lab;
using UltimateERP.Domain.Entities.HMS;
using UltimateERP.Domain.Entities.Service;
using UltimateERP.Domain.Entities.TaskModule;
using UltimateERP.Domain.Entities.Finance;
using UltimateERP.Domain.Entities.CMS;
using UltimateERP.Domain.Entities.Support;
using UltimateERP.Domain.Entities.Loyalty;
using UltimateERP.Domain.Entities.KYC;
using UltimateERP.Domain.Entities.IndustrySpecific;
using UltimateERP.Domain.Entities.Reporting;
using UltimateERP.Domain.Entities.Jobs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.Persistence;

public class ERPDbContext : DbContext, IApplicationDbContext
{
    public ERPDbContext(DbContextOptions<ERPDbContext> options) : base(options) { }

    // ── Setup ────────────────────────────────────────────────────────────
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<CostClass> CostClasses => Set<CostClass>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<EntityNumbering> EntityNumberings => Set<EntityNumbering>();
    public DbSet<UserDefinedField> UserDefinedFields => Set<UserDefinedField>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

    // ── Security ─────────────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<UserGroupMember> UserGroupMembers => Set<UserGroupMember>();
    public DbSet<EntityPermission> EntityPermissions => Set<EntityPermission>();
    public DbSet<ModuleAccess> ModuleAccesses => Set<ModuleAccess>();
    public DbSet<BranchAccess> BranchAccesses => Set<BranchAccess>();
    public DbSet<GodownAccess> GodownAccesses => Set<GodownAccess>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // ── Account ──────────────────────────────────────────────────────────
    public DbSet<LedgerGroup> LedgerGroups => Set<LedgerGroup>();
    public DbSet<Ledger> Ledgers => Set<Ledger>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<VoucherDetail> VoucherDetails => Set<VoucherDetail>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PDC> PDCs => Set<PDC>();
    public DbSet<ODC> ODCs => Set<ODC>();
    public DbSet<BankGuarantee> BankGuarantees => Set<BankGuarantee>();
    public DbSet<LetterOfCredit> LettersOfCredit => Set<LetterOfCredit>();
    public DbSet<BankReconciliation> BankReconciliations => Set<BankReconciliation>();
    public DbSet<FiscalYear> FiscalYears => Set<FiscalYear>();
    public DbSet<PaymentTerm> PaymentTerms => Set<PaymentTerm>();
    public DbSet<PaymentMode> PaymentModes => Set<PaymentMode>();

    // ── Inventory ────────────────────────────────────────────────────────
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Godown> Godowns => Set<Godown>();
    public DbSet<Rack> Racks => Set<Rack>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails => Set<PurchaseInvoiceDetail>();
    public DbSet<PurchaseQuotation> PurchaseQuotations => Set<PurchaseQuotation>();
    public DbSet<PurchaseQuotationDetail> PurchaseQuotationDetails => Set<PurchaseQuotationDetail>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
    public DbSet<ReceiptNote> ReceiptNotes => Set<ReceiptNote>();
    public DbSet<ReceiptNoteDetail> ReceiptNoteDetails => Set<ReceiptNoteDetail>();
    public DbSet<PurchaseReturn> PurchaseReturns => Set<PurchaseReturn>();
    public DbSet<PurchaseReturnDetail> PurchaseReturnDetails => Set<PurchaseReturnDetail>();
    public DbSet<PurchaseDebitNote> PurchaseDebitNotes => Set<PurchaseDebitNote>();
    public DbSet<PurchaseCreditNote> PurchaseCreditNotes => Set<PurchaseCreditNote>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceDetail> SalesInvoiceDetails => Set<SalesInvoiceDetail>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<StockJournal> StockJournals => Set<StockJournal>();
    public DbSet<ProductionOrder> ProductionOrders => Set<ProductionOrder>();
    public DbSet<Indent> Indents => Set<Indent>();
    public DbSet<IndentDetail> IndentDetails => Set<IndentDetail>();
    public DbSet<StockDemand> StockDemands => Set<StockDemand>();
    public DbSet<DispatchOrder> DispatchOrders => Set<DispatchOrder>();
    public DbSet<DispatchSection> DispatchSections => Set<DispatchSection>();
    public DbSet<GatePass> GatePasses => Set<GatePass>();
    public DbSet<SalesAllotment> SalesAllotments => Set<SalesAllotment>();
    public DbSet<SalesQuotation> SalesQuotations => Set<SalesQuotation>();
    public DbSet<SalesQuotationDetail> SalesQuotationDetails => Set<SalesQuotationDetail>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderDetail> SalesOrderDetails => Set<SalesOrderDetail>();
    public DbSet<SalesReturn> SalesReturns => Set<SalesReturn>();
    public DbSet<SalesReturnDetail> SalesReturnDetails => Set<SalesReturnDetail>();
    public DbSet<SalesDebitNote> SalesDebitNotes => Set<SalesDebitNote>();
    public DbSet<SalesCreditNote> SalesCreditNotes => Set<SalesCreditNote>();
    public DbSet<LandedCost> LandedCosts => Set<LandedCost>();
    public DbSet<BOM> BOMs => Set<BOM>();
    public DbSet<BOMDetail> BOMDetails => Set<BOMDetail>();

    // ── HR ────────────────────────────────────────────────────────────────
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Leave> Leaves => Set<Leave>();
    public DbSet<ExpenseClaim> ExpenseClaims => Set<ExpenseClaim>();

    // ── Assets ───────────────────────────────────────────────────────────
    public DbSet<AssetMaster> Assets => Set<AssetMaster>();
    public DbSet<AssetIssue> AssetIssues => Set<AssetIssue>();
    public DbSet<AssetTransfer> AssetTransfers => Set<AssetTransfer>();

    // ── Lab ──────────────────────────────────────────────────────────────
    public DbSet<SampleCollection> SampleCollections => Set<SampleCollection>();
    public DbSet<LabReport> LabReports => Set<LabReport>();

    // ── HMS ──────────────────────────────────────────────────────────────
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<OPDTicket> OPDTickets => Set<OPDTicket>();
    public DbSet<IPDAdmission> IPDAdmissions => Set<IPDAdmission>();
    public DbSet<Bed> Beds => Set<Bed>();

    // ── Service ──────────────────────────────────────────────────────────
    public DbSet<ComplaintTicket> ComplaintTickets => Set<ComplaintTicket>();
    public DbSet<JobCard> JobCards => Set<JobCard>();
    public DbSet<SparePartsDemand> SparePartsDemands => Set<SparePartsDemand>();
    public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
    public DbSet<DeviceModel> DeviceModels => Set<DeviceModel>();
    public DbSet<ServiceAppointment> ServiceAppointments => Set<ServiceAppointment>();

    // ── Task ─────────────────────────────────────────────────────────────
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();

    // ── Finance──────────────────────────────────────────────────────────
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<LoanEMI> LoanEMIs => Set<LoanEMI>();
    public DbSet<VehicleDetail> VehicleDetails => Set<VehicleDetail>();

    // ── CMS ──────────────────────────────────────────────────────────────
    public DbSet<Slider> Sliders => Set<Slider>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<Notice> Notices => Set<Notice>();

    // ── Support ──────────────────────────────────────────────────────────
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    // ── Loyalty ──────────────────────────────────────────────────────────
    public DbSet<MembershipPoint> MembershipPoints => Set<MembershipPoint>();

    // ── KYC ──────────────────────────────────────────────────────────────
    public DbSet<KYCRecord> KYCRecords => Set<KYCRecord>();

    // ── Industry-Specific ────────────────────────────────────────────────
    public DbSet<FixedProductConfig> FixedProductConfigs => Set<FixedProductConfig>();
    public DbSet<DairyPurchaseInvoice> DairyPurchaseInvoices => Set<DairyPurchaseInvoice>();
    public DbSet<DairyPurchaseDetail> DairyPurchaseDetails => Set<DairyPurchaseDetail>();
    public DbSet<DairySalesInvoice> DairySalesInvoices => Set<DairySalesInvoice>();
    public DbSet<TeaPurchaseInvoice> TeaPurchaseInvoices => Set<TeaPurchaseInvoice>();
    public DbSet<PetrolPumpTransaction> PetrolPumpTransactions => Set<PetrolPumpTransaction>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();

    // ── Reporting ────────────────────────────────────────────────────────
    public DbSet<ReportWriterDefinition> ReportWriterDefinitions => Set<ReportWriterDefinition>();
    public DbSet<DynamicAIDashboard> DynamicAIDashboards => Set<DynamicAIDashboard>();
    public DbSet<CustomDashboard> CustomDashboards => Set<CustomDashboard>();
    public DbSet<DashboardWidget> DashboardWidgets => Set<DashboardWidget>();

    // ── Jobs ─────────────────────────────────────────────────────────────
    public DbSet<ScheduledJobConfig> ScheduledJobConfigs => Set<ScheduledJobConfig>();
    public DbSet<JobExecutionLog> JobExecutionLogs => Set<JobExecutionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ERPDbContext).Assembly);

        // Global soft-delete filter for all BaseEntity-derived types
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Domain.Common.BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(Domain.Common.BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
