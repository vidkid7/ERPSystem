using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Services;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Commands;

// ── LedgerGroup Commands ──────────────────────────────────────────────

public record CreateLedgerGroupCommand(CreateLedgerGroupDto Group) : IRequest<ApiResponse<LedgerGroupDto>>;

public class CreateLedgerGroupValidator : AbstractValidator<CreateLedgerGroupCommand>
{
    public CreateLedgerGroupValidator()
    {
        RuleFor(x => x.Group.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Group.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateLedgerGroupHandler : IRequestHandler<CreateLedgerGroupCommand, ApiResponse<LedgerGroupDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateLedgerGroupHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LedgerGroupDto>> Handle(CreateLedgerGroupCommand request, CancellationToken ct)
    {
        if (await _db.LedgerGroups.AnyAsync(g => g.Code == request.Group.Code, ct))
            return ApiResponse<LedgerGroupDto>.Failure($"Ledger group code '{request.Group.Code}' already exists");

        if (request.Group.ParentGroupId.HasValue)
        {
            var parent = await _db.LedgerGroups.FindAsync(new object[] { request.Group.ParentGroupId.Value }, ct);
            if (parent is null) return ApiResponse<LedgerGroupDto>.Failure("Parent group not found");
        }

        var entity = _mapper.Map<LedgerGroup>(request.Group);
        _db.LedgerGroups.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LedgerGroupDto>.Success(_mapper.Map<LedgerGroupDto>(entity), "Ledger group created");
    }
}

// ── Ledger Commands ───────────────────────────────────────────────────

public record CreateLedgerCommand(CreateLedgerDto Ledger) : IRequest<ApiResponse<LedgerDto>>;

public class CreateLedgerValidator : AbstractValidator<CreateLedgerCommand>
{
    public CreateLedgerValidator()
    {
        RuleFor(x => x.Ledger.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Ledger.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Ledger.LedgerGroupId).GreaterThan(0);
    }
}

public class CreateLedgerHandler : IRequestHandler<CreateLedgerCommand, ApiResponse<LedgerDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateLedgerHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LedgerDto>> Handle(CreateLedgerCommand request, CancellationToken ct)
    {
        if (await _db.Ledgers.AnyAsync(l => l.Code == request.Ledger.Code, ct))
            return ApiResponse<LedgerDto>.Failure($"Ledger code '{request.Ledger.Code}' already exists");

        var group = await _db.LedgerGroups.FindAsync(new object[] { request.Ledger.LedgerGroupId }, ct);
        if (group is null) return ApiResponse<LedgerDto>.Failure("Ledger group not found");

        var entity = _mapper.Map<Ledger>(request.Ledger);
        _db.Ledgers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LedgerDto>.Success(_mapper.Map<LedgerDto>(entity), "Ledger created");
    }
}

// ── Voucher Commands ──────────────────────────────────────────────────

public record CreateVoucherCommand(CreateVoucherDto Voucher) : IRequest<ApiResponse<VoucherDto>>;

public class CreateVoucherValidator : AbstractValidator<CreateVoucherCommand>
{
    public CreateVoucherValidator()
    {
        RuleFor(x => x.Voucher.Details).NotEmpty().WithMessage("Voucher must have at least one detail line");
        RuleForEach(x => x.Voucher.Details).ChildRules(detail =>
        {
            detail.RuleFor(d => d.LedgerId).GreaterThan(0);
            detail.RuleFor(d => d)
                .Must(d => d.DebitAmount > 0 || d.CreditAmount > 0)
                .WithMessage("Each line must have either debit or credit amount");
        });
    }
}

public class CreateVoucherHandler : IRequestHandler<CreateVoucherCommand, ApiResponse<VoucherDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateVoucherHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<VoucherDto>> Handle(CreateVoucherCommand request, CancellationToken ct)
    {
        var totalDebit = request.Voucher.Details.Sum(d => d.DebitAmount);
        var totalCredit = request.Voucher.Details.Sum(d => d.CreditAmount);

        var (isValid, error) = VoucherValidator.ValidateBalance(totalDebit, totalCredit);
        if (!isValid) return ApiResponse<VoucherDto>.Failure(error!);

        // Validate all ledger IDs exist
        var ledgerIds = request.Voucher.Details.Select(d => d.LedgerId).Distinct().ToList();
        var existingLedgerCount = await _db.Ledgers.CountAsync(l => ledgerIds.Contains(l.Id), ct);
        if (existingLedgerCount != ledgerIds.Count)
            return ApiResponse<VoucherDto>.Failure("One or more ledger IDs are invalid");

        var voucher = new Voucher
        {
            VoucherDate = request.Voucher.VoucherDate,
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("V-", 1),
            CommonNarration = request.Voucher.Narration,
            TotalDebit = totalDebit,
            TotalCredit = totalCredit,
            IsPosted = false
        };

        int lineNum = 1;
        foreach (var detail in request.Voucher.Details)
        {
            voucher.Details.Add(new VoucherDetail
            {
                LineNumber = lineNum++,
                LedgerId = detail.LedgerId,
                DebitAmount = detail.DebitAmount,
                CreditAmount = detail.CreditAmount,
                Narration = detail.Narration
            });
        }

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var savedVoucher = await _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .FirstAsync(v => v.Id == voucher.Id, ct);

        return ApiResponse<VoucherDto>.Success(_mapper.Map<VoucherDto>(savedVoucher), "Voucher created");
    }
}

// ── Customer Commands ─────────────────────────────────────────────────

public record CreateCustomerCommand(CreateCustomerDto Customer) : IRequest<ApiResponse<CustomerDto>>;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Customer.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Customer.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Customer.CreditLimit).GreaterThanOrEqualTo(0);
    }
}

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateCustomerHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        if (await _db.Customers.AnyAsync(c => c.Code == request.Customer.Code, ct))
            return ApiResponse<CustomerDto>.Failure($"Customer code '{request.Customer.Code}' already exists");

        // Auto-create a Sundry Debtors ledger for the customer
        var ledger = new Ledger
        {
            Code = $"C-{request.Customer.Code}",
            Name = request.Customer.Name,
            OpeningBalance = 0
        };
        _db.Ledgers.Add(ledger);
        await _db.SaveChangesAsync(ct);

        var customer = _mapper.Map<Customer>(request.Customer);
        customer.LedgerId = ledger.Id;
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer), "Customer created");
    }
}

// ── Vendor Commands ───────────────────────────────────────────────────

public record CreateVendorCommand(CreateVendorDto Vendor) : IRequest<ApiResponse<VendorDto>>;

public class CreateVendorHandler : IRequestHandler<CreateVendorCommand, ApiResponse<VendorDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateVendorHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<VendorDto>> Handle(CreateVendorCommand request, CancellationToken ct)
    {
        if (await _db.Vendors.AnyAsync(v => v.Code == request.Vendor.Code, ct))
            return ApiResponse<VendorDto>.Failure($"Vendor code '{request.Vendor.Code}' already exists");

        // Auto-create a Sundry Creditors ledger for the vendor
        var ledger = new Ledger
        {
            Code = $"V-{request.Vendor.Code}",
            Name = request.Vendor.Name,
            OpeningBalance = 0
        };
        _db.Ledgers.Add(ledger);
        await _db.SaveChangesAsync(ct);

        var vendor = _mapper.Map<Vendor>(request.Vendor);
        vendor.LedgerId = ledger.Id;
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<VendorDto>.Success(_mapper.Map<VendorDto>(vendor), "Vendor created");
    }
}
