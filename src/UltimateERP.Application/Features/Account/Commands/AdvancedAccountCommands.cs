using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Account.Commands;

// ── PDC Commands ──────────────────────────────────────────────────────

public record CreatePDCCommand(CreatePDCDto PDC) : IRequest<ApiResponse<PDCDto>>;

public class CreatePDCValidator : AbstractValidator<CreatePDCCommand>
{
    public CreatePDCValidator()
    {
        RuleFor(x => x.PDC.ChequeNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PDC.Amount).GreaterThan(0);
        RuleFor(x => x.PDC.LedgerId).GreaterThan(0);
        RuleFor(x => x.PDC.VoucherId).GreaterThan(0);
    }
}

public class CreatePDCHandler : IRequestHandler<CreatePDCCommand, ApiResponse<PDCDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePDCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PDCDto>> Handle(CreatePDCCommand request, CancellationToken ct)
    {
        var voucher = await _db.Vouchers.FindAsync(new object[] { request.PDC.VoucherId }, ct);
        if (voucher is null) return ApiResponse<PDCDto>.Failure("Voucher not found");

        var ledger = await _db.Ledgers.FindAsync(new object[] { request.PDC.LedgerId }, ct);
        if (ledger is null) return ApiResponse<PDCDto>.Failure("Ledger not found");

        var entity = _mapper.Map<PDC>(request.PDC);
        entity.Status = PDCStatus.Pending;
        _db.PDCs.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<PDCDto>.Success(_mapper.Map<PDCDto>(entity), "PDC created");
    }
}

public record UpdatePDCStatusCommand(int Id, PDCStatus Status) : IRequest<ApiResponse<PDCDto>>;

public class UpdatePDCStatusHandler : IRequestHandler<UpdatePDCStatusCommand, ApiResponse<PDCDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdatePDCStatusHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PDCDto>> Handle(UpdatePDCStatusCommand request, CancellationToken ct)
    {
        var entity = await _db.PDCs.Include(p => p.Ledger).FirstOrDefaultAsync(p => p.Id == request.Id, ct);
        if (entity is null) return ApiResponse<PDCDto>.Failure("PDC not found");

        entity.Status = request.Status;
        if (request.Status == PDCStatus.Cleared)
            entity.ClearedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<PDCDto>.Success(_mapper.Map<PDCDto>(entity), "PDC status updated");
    }
}

// ── ODC Commands ──────────────────────────────────────────────────────

public record CreateODCCommand(CreateODCDto ODC) : IRequest<ApiResponse<ODCDto>>;

public class CreateODCValidator : AbstractValidator<CreateODCCommand>
{
    public CreateODCValidator()
    {
        RuleFor(x => x.ODC.ChequeNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ODC.Amount).GreaterThan(0);
        RuleFor(x => x.ODC.LedgerId).GreaterThan(0);
        RuleFor(x => x.ODC.VoucherId).GreaterThan(0);
    }
}

public class CreateODCHandler : IRequestHandler<CreateODCCommand, ApiResponse<ODCDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateODCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ODCDto>> Handle(CreateODCCommand request, CancellationToken ct)
    {
        var voucher = await _db.Vouchers.FindAsync(new object[] { request.ODC.VoucherId }, ct);
        if (voucher is null) return ApiResponse<ODCDto>.Failure("Voucher not found");

        var ledger = await _db.Ledgers.FindAsync(new object[] { request.ODC.LedgerId }, ct);
        if (ledger is null) return ApiResponse<ODCDto>.Failure("Ledger not found");

        var entity = _mapper.Map<ODC>(request.ODC);
        entity.Status = "Pending";
        _db.ODCs.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ODCDto>.Success(_mapper.Map<ODCDto>(entity), "ODC created");
    }
}

public record UpdateODCStatusCommand(int Id, string Status) : IRequest<ApiResponse<ODCDto>>;

public class UpdateODCStatusHandler : IRequestHandler<UpdateODCStatusCommand, ApiResponse<ODCDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateODCStatusHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ODCDto>> Handle(UpdateODCStatusCommand request, CancellationToken ct)
    {
        var entity = await _db.ODCs.Include(o => o.Ledger).FirstOrDefaultAsync(o => o.Id == request.Id, ct);
        if (entity is null) return ApiResponse<ODCDto>.Failure("ODC not found");

        entity.Status = request.Status;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ODCDto>.Success(_mapper.Map<ODCDto>(entity), "ODC status updated");
    }
}

// ── Bank Guarantee Commands ───────────────────────────────────────────

public record CreateBankGuaranteeCommand(CreateBankGuaranteeDto BankGuarantee) : IRequest<ApiResponse<BankGuaranteeDto>>;

public class CreateBankGuaranteeValidator : AbstractValidator<CreateBankGuaranteeCommand>
{
    public CreateBankGuaranteeValidator()
    {
        RuleFor(x => x.BankGuarantee.GuaranteeNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BankGuarantee.GuaranteeAmount).GreaterThan(0);
        RuleFor(x => x.BankGuarantee.LedgerId).GreaterThan(0);
        RuleFor(x => x.BankGuarantee.ValidTo).GreaterThan(x => x.BankGuarantee.ValidFrom);
    }
}

public class CreateBankGuaranteeHandler : IRequestHandler<CreateBankGuaranteeCommand, ApiResponse<BankGuaranteeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateBankGuaranteeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BankGuaranteeDto>> Handle(CreateBankGuaranteeCommand request, CancellationToken ct)
    {
        var ledger = await _db.Ledgers.FindAsync(new object[] { request.BankGuarantee.LedgerId }, ct);
        if (ledger is null) return ApiResponse<BankGuaranteeDto>.Failure("Ledger not found");

        var entity = _mapper.Map<BankGuarantee>(request.BankGuarantee);
        _db.BankGuarantees.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<BankGuaranteeDto>.Success(_mapper.Map<BankGuaranteeDto>(entity), "Bank guarantee created");
    }
}

// ── Letter of Credit Commands ─────────────────────────────────────────

public record CreateLetterOfCreditCommand(CreateLetterOfCreditDto LetterOfCredit) : IRequest<ApiResponse<LetterOfCreditDto>>;

public class CreateLetterOfCreditValidator : AbstractValidator<CreateLetterOfCreditCommand>
{
    public CreateLetterOfCreditValidator()
    {
        RuleFor(x => x.LetterOfCredit.LCNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LetterOfCredit.AmountInFC).GreaterThan(0);
        RuleFor(x => x.LetterOfCredit.ExpiryDate).GreaterThan(x => x.LetterOfCredit.OpeningDate);
    }
}

public class CreateLetterOfCreditHandler : IRequestHandler<CreateLetterOfCreditCommand, ApiResponse<LetterOfCreditDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateLetterOfCreditHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LetterOfCreditDto>> Handle(CreateLetterOfCreditCommand request, CancellationToken ct)
    {
        var entity = _mapper.Map<LetterOfCredit>(request.LetterOfCredit);
        entity.Status = LCStatus.Open;
        _db.LettersOfCredit.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LetterOfCreditDto>.Success(_mapper.Map<LetterOfCreditDto>(entity), "Letter of credit created");
    }
}

// ── Bank Reconciliation Commands ──────────────────────────────────────

public record ReconcileTransactionCommand(ReconcileTransactionDto Reconciliation) : IRequest<ApiResponse<BankReconciliationDto>>;

public class ReconcileTransactionHandler : IRequestHandler<ReconcileTransactionCommand, ApiResponse<BankReconciliationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ReconcileTransactionHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BankReconciliationDto>> Handle(ReconcileTransactionCommand request, CancellationToken ct)
    {
        var entity = await _db.BankReconciliations
            .Include(r => r.Ledger)
            .FirstOrDefaultAsync(r => r.Id == request.Reconciliation.Id, ct);

        if (entity is null) return ApiResponse<BankReconciliationDto>.Failure("Transaction not found");
        if (entity.IsReconciled) return ApiResponse<BankReconciliationDto>.Failure("Transaction already reconciled");

        entity.BankDate = request.Reconciliation.BankDate;
        entity.IsReconciled = true;
        entity.ReconciledDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<BankReconciliationDto>.Success(_mapper.Map<BankReconciliationDto>(entity), "Transaction reconciled");
    }
}
