using Microsoft.EntityFrameworkCore;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Entities.Setup;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ERPDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        await SeedCurrenciesAsync(context);
        await SeedBranchesAsync(context);
        await SeedFiscalYearsAsync(context);
        await SeedLedgerGroupsAsync(context);
        await SeedProductGroupsAsync(context);
        await SeedGodownsAsync(context);
    }

    private static async Task SeedCurrenciesAsync(ERPDbContext context)
    {
        if (await context.Currencies.AnyAsync()) return;

        context.Currencies.AddRange(
            new Currency { Code = "USD", Name = "US Dollar", CurrencyCode = "USD", Symbol = "$", DecimalPlaces = 2, IsBaseCurrency = true },
            new Currency { Code = "EUR", Name = "Euro", CurrencyCode = "EUR", Symbol = "€", DecimalPlaces = 2 },
            new Currency { Code = "GBP", Name = "British Pound", CurrencyCode = "GBP", Symbol = "£", DecimalPlaces = 2 },
            new Currency { Code = "NPR", Name = "Nepalese Rupee", CurrencyCode = "NPR", Symbol = "Rs.", DecimalPlaces = 2 },
            new Currency { Code = "INR", Name = "Indian Rupee", CurrencyCode = "INR", Symbol = "₹", DecimalPlaces = 2 }
        );
        await context.SaveChangesAsync();
    }

    private static async Task SeedBranchesAsync(ERPDbContext context)
    {
        if (await context.Branches.AnyAsync()) return;

        context.Branches.AddRange(
            new Branch
            {
                Code = "HO",
                Name = "Head Office",
                Address = "123 Main Street",
                City = "Kathmandu",
                State = "Bagmati",
                Country = "Nepal",
                Phone = "+977-1-4000000",
                Email = "headoffice@erp.com",
                IsHeadOffice = true
            },
            new Branch
            {
                Code = "BR1",
                Name = "Branch 1",
                Address = "456 Park Road",
                City = "Pokhara",
                State = "Gandaki",
                Country = "Nepal",
                Phone = "+977-61-400000",
                Email = "branch1@erp.com"
            },
            new Branch
            {
                Code = "BR2",
                Name = "Branch 2",
                Address = "789 Lake View",
                City = "Biratnagar",
                State = "Koshi",
                Country = "Nepal",
                Phone = "+977-21-400000",
                Email = "branch2@erp.com"
            }
        );
        await context.SaveChangesAsync();
    }

    private static async Task SeedFiscalYearsAsync(ERPDbContext context)
    {
        if (await context.FiscalYears.AnyAsync()) return;

        context.FiscalYears.AddRange(
            new FiscalYear
            {
                Code = "FY2023",
                Name = "Fiscal Year 2023-24",
                StartDate = new DateTime(2023, 4, 1),
                EndDate = new DateTime(2024, 3, 31),
                Status = FiscalYearStatus.Closed,
                IsCurrent = false
            },
            new FiscalYear
            {
                Code = "FY2024",
                Name = "Fiscal Year 2024-25",
                StartDate = new DateTime(2024, 4, 1),
                EndDate = new DateTime(2025, 3, 31),
                Status = FiscalYearStatus.Open,
                IsCurrent = true
            }
        );
        await context.SaveChangesAsync();
    }

    private static async Task SeedLedgerGroupsAsync(ERPDbContext context)
    {
        if (await context.LedgerGroups.AnyAsync()) return;

        // Root groups
        var assets = new LedgerGroup
        {
            Code = "ASSETS",
            Name = "Assets",
            NatureOfGroup = NatureOfGroup.Asset,
            TypeOfGroup = "Balance Sheet",
            ShowInLedgerMaster = true,
            NumberingMethod = NumberingMethod.Automatic,
            NumericalPartWidth = 4,
            StartNumber = 1,
            InBuilt = true
        };
        var liabilities = new LedgerGroup
        {
            Code = "LIAB",
            Name = "Liabilities",
            NatureOfGroup = NatureOfGroup.Liability,
            TypeOfGroup = "Balance Sheet",
            ShowInLedgerMaster = true,
            NumberingMethod = NumberingMethod.Automatic,
            NumericalPartWidth = 4,
            StartNumber = 1,
            InBuilt = true
        };
        var income = new LedgerGroup
        {
            Code = "INCOME",
            Name = "Income",
            NatureOfGroup = NatureOfGroup.Income,
            TypeOfGroup = "P&L",
            ShowInLedgerMaster = true,
            NumberingMethod = NumberingMethod.Automatic,
            NumericalPartWidth = 4,
            StartNumber = 1,
            InBuilt = true
        };
        var expense = new LedgerGroup
        {
            Code = "EXPENSE",
            Name = "Expenses",
            NatureOfGroup = NatureOfGroup.Expense,
            TypeOfGroup = "P&L",
            ShowInLedgerMaster = true,
            NumberingMethod = NumberingMethod.Automatic,
            NumericalPartWidth = 4,
            StartNumber = 1,
            InBuilt = true
        };

        context.LedgerGroups.AddRange(assets, liabilities, income, expense);
        await context.SaveChangesAsync();

        // Child groups
        context.LedgerGroups.AddRange(
            new LedgerGroup
            {
                Code = "CURR_ASSETS",
                Name = "Current Assets",
                ParentGroupId = assets.Id,
                NatureOfGroup = NatureOfGroup.Asset,
                TypeOfGroup = "Balance Sheet",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "FIXED_ASSETS",
                Name = "Fixed Assets",
                ParentGroupId = assets.Id,
                NatureOfGroup = NatureOfGroup.Asset,
                TypeOfGroup = "Balance Sheet",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "BANK_ACCOUNTS",
                Name = "Bank Accounts",
                ParentGroupId = assets.Id,
                NatureOfGroup = NatureOfGroup.Asset,
                TypeOfGroup = "Balance Sheet",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "SUNDRY_DEBTORS",
                Name = "Sundry Debtors",
                ParentGroupId = assets.Id,
                NatureOfGroup = NatureOfGroup.Asset,
                TypeOfGroup = "Balance Sheet",
                IsDebtor = true,
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "SUNDRY_CREDITORS",
                Name = "Sundry Creditors",
                ParentGroupId = liabilities.Id,
                NatureOfGroup = NatureOfGroup.Liability,
                TypeOfGroup = "Balance Sheet",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "CAPITAL",
                Name = "Capital Account",
                ParentGroupId = liabilities.Id,
                NatureOfGroup = NatureOfGroup.Liability,
                TypeOfGroup = "Balance Sheet",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "SALES",
                Name = "Sales",
                ParentGroupId = income.Id,
                NatureOfGroup = NatureOfGroup.Income,
                TypeOfGroup = "P&L",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "PURCHASE",
                Name = "Purchase",
                ParentGroupId = expense.Id,
                NatureOfGroup = NatureOfGroup.Expense,
                TypeOfGroup = "P&L",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            },
            new LedgerGroup
            {
                Code = "INDIRECT_EXP",
                Name = "Indirect Expenses",
                ParentGroupId = expense.Id,
                NatureOfGroup = NatureOfGroup.Expense,
                TypeOfGroup = "P&L",
                ShowInLedgerMaster = true,
                NumberingMethod = NumberingMethod.Automatic,
                NumericalPartWidth = 4,
                StartNumber = 1
            }
        );
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductGroupsAsync(ERPDbContext context)
    {
        if (await context.ProductGroups.AnyAsync()) return;

        var electronics = new ProductGroup { Code = "ELEC", Name = "Electronics" };
        var furniture = new ProductGroup { Code = "FURN", Name = "Furniture" };
        var consumables = new ProductGroup { Code = "CONS", Name = "Consumables" };
        var rawMaterials = new ProductGroup { Code = "RAW", Name = "Raw Materials" };
        var finishedGoods = new ProductGroup { Code = "FG", Name = "Finished Goods" };

        context.ProductGroups.AddRange(electronics, furniture, consumables, rawMaterials, finishedGoods);
        await context.SaveChangesAsync();

        context.ProductGroups.AddRange(
            new ProductGroup { Code = "MOBILE", Name = "Mobile Phones", ParentGroupId = electronics.Id },
            new ProductGroup { Code = "LAPTOP", Name = "Laptops", ParentGroupId = electronics.Id },
            new ProductGroup { Code = "OFFICE_FURN", Name = "Office Furniture", ParentGroupId = furniture.Id }
        );
        await context.SaveChangesAsync();
    }

    private static async Task SeedGodownsAsync(ERPDbContext context)
    {
        if (await context.Godowns.AnyAsync()) return;

        context.Godowns.AddRange(
            new Godown { Code = "MAIN", Name = "Main Warehouse", Address = "Warehouse Complex, Kathmandu", GodownType = "Main" },
            new Godown { Code = "STORE1", Name = "Store 1", Address = "Branch 1, Pokhara", GodownType = "Branch" },
            new Godown { Code = "STORE2", Name = "Store 2", Address = "Branch 2, Biratnagar", GodownType = "Branch" }
        );
        await context.SaveChangesAsync();
    }
}
