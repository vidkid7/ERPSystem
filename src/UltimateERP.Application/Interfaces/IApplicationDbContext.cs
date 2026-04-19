using Microsoft.EntityFrameworkCore;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Domain.Entities.Setup;

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

    // Setup
    DbSet<Branch> Branches { get; }
    DbSet<CostClass> CostClasses { get; }
    DbSet<DocumentType> DocumentTypes { get; }
    DbSet<EntityNumbering> EntityNumberings { get; }

    // Account
    DbSet<Ledger> Ledgers { get; }
    DbSet<LedgerGroup> LedgerGroups { get; }
    DbSet<Voucher> Vouchers { get; }
    DbSet<VoucherDetail> VoucherDetails { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vendor> Vendors { get; }

    // Inventory
    DbSet<Product> Products { get; }
    DbSet<ProductGroup> ProductGroups { get; }
    DbSet<Godown> Godowns { get; }
    DbSet<Stock> Stocks { get; }
    DbSet<PurchaseInvoice> PurchaseInvoices { get; }
    DbSet<SalesInvoice> SalesInvoices { get; }

    // HR
    DbSet<Employee> Employees { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<Leave> Leaves { get; }
    DbSet<ExpenseClaim> ExpenseClaims { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
