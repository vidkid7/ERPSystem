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

// ── Receipt Voucher ──────────────────────────────────────────────────

public record CreateReceiptVoucherCommand(CreateReceiptVoucherDto Dto) : IRequest<ApiResponse<ReceiptVoucherDto>>;

public class CreateReceiptVoucherValidator : AbstractValidator<CreateReceiptVoucherCommand>
{
    public CreateReceiptVoucherValidator()
    {
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.BankLedgerId).GreaterThan(0);
        RuleFor(x => x.Dto.PartyLedgerId).GreaterThan(0);
    }
}

public class CreateReceiptVoucherHandler : IRequestHandler<CreateReceiptVoucherCommand, ApiResponse<ReceiptVoucherDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateReceiptVoucherHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ReceiptVoucherDto>> Handle(CreateReceiptVoucherCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var bankLedger = await _db.Ledgers.FindAsync(new object[] { dto.BankLedgerId }, ct);
        if (bankLedger is null) return ApiResponse<ReceiptVoucherDto>.Failure("Bank/Cash ledger not found");

        var partyLedger = await _db.Ledgers.FindAsync(new object[] { dto.PartyLedgerId }, ct);
        if (partyLedger is null) return ApiResponse<ReceiptVoucherDto>.Failure("Party ledger not found");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("RV-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Narration,
            ReferenceNumber = dto.ChequeNo,
            TotalDebit = dto.Amount,
            TotalCredit = dto.Amount,
            VoucherTypeId = 1 // Receipt
        };

        // Debit Bank/Cash, Credit Party
        voucher.Details.Add(new VoucherDetail { LineNumber = 1, LedgerId = dto.BankLedgerId, DebitAmount = dto.Amount, CreditAmount = 0, Narration = $"Received from {dto.ReceivedFrom}" });
        voucher.Details.Add(new VoucherDetail { LineNumber = 2, LedgerId = dto.PartyLedgerId, DebitAmount = 0, CreditAmount = dto.Amount, Narration = $"Received from {dto.ReceivedFrom}" });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.Vouchers.Include(v => v.Details).ThenInclude(d => d.Ledger).FirstAsync(v => v.Id == voucher.Id, ct);
        var result = new ReceiptVoucherDto
        {
            Id = saved.Id,
            VoucherNo = saved.VoucherNumber,
            Date = saved.VoucherDate,
            ReceivedFrom = dto.ReceivedFrom,
            Amount = dto.Amount,
            BankLedgerId = dto.BankLedgerId,
            BankLedgerName = bankLedger.Name,
            PartyLedgerId = dto.PartyLedgerId,
            PartyLedgerName = partyLedger.Name,
            PaymentMode = dto.PaymentMode,
            ChequeNo = dto.ChequeNo,
            Narration = saved.CommonNarration
        };

        return ApiResponse<ReceiptVoucherDto>.Success(result, "Receipt voucher created");
    }
}

// ── Payment Voucher ──────────────────────────────────────────────────

public record CreatePaymentVoucherCommand(CreatePaymentVoucherDto Dto) : IRequest<ApiResponse<PaymentVoucherDto>>;

public class CreatePaymentVoucherValidator : AbstractValidator<CreatePaymentVoucherCommand>
{
    public CreatePaymentVoucherValidator()
    {
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.BankLedgerId).GreaterThan(0);
        RuleFor(x => x.Dto.PartyLedgerId).GreaterThan(0);
    }
}

public class CreatePaymentVoucherHandler : IRequestHandler<CreatePaymentVoucherCommand, ApiResponse<PaymentVoucherDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePaymentVoucherHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentVoucherDto>> Handle(CreatePaymentVoucherCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var bankLedger = await _db.Ledgers.FindAsync(new object[] { dto.BankLedgerId }, ct);
        if (bankLedger is null) return ApiResponse<PaymentVoucherDto>.Failure("Bank/Cash ledger not found");

        var partyLedger = await _db.Ledgers.FindAsync(new object[] { dto.PartyLedgerId }, ct);
        if (partyLedger is null) return ApiResponse<PaymentVoucherDto>.Failure("Party ledger not found");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("PV-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Narration,
            ReferenceNumber = dto.ChequeNo,
            TotalDebit = dto.Amount,
            TotalCredit = dto.Amount,
            VoucherTypeId = 2 // Payment
        };

        // Debit Party, Credit Bank/Cash
        voucher.Details.Add(new VoucherDetail { LineNumber = 1, LedgerId = dto.PartyLedgerId, DebitAmount = dto.Amount, CreditAmount = 0, Narration = $"Paid to {dto.PaidTo}" });
        voucher.Details.Add(new VoucherDetail { LineNumber = 2, LedgerId = dto.BankLedgerId, DebitAmount = 0, CreditAmount = dto.Amount, Narration = $"Paid to {dto.PaidTo}" });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.Vouchers.Include(v => v.Details).ThenInclude(d => d.Ledger).FirstAsync(v => v.Id == voucher.Id, ct);
        var result = new PaymentVoucherDto
        {
            Id = saved.Id,
            VoucherNo = saved.VoucherNumber,
            Date = saved.VoucherDate,
            PaidTo = dto.PaidTo,
            Amount = dto.Amount,
            BankLedgerId = dto.BankLedgerId,
            BankLedgerName = bankLedger.Name,
            PartyLedgerId = dto.PartyLedgerId,
            PartyLedgerName = partyLedger.Name,
            PaymentMode = dto.PaymentMode,
            ChequeNo = dto.ChequeNo,
            Narration = saved.CommonNarration
        };

        return ApiResponse<PaymentVoucherDto>.Success(result, "Payment voucher created");
    }
}

// ── Journal Voucher ──────────────────────────────────────────────────

public record CreateJournalVoucherCommand(CreateJournalVoucherDto Dto) : IRequest<ApiResponse<JournalVoucherDto>>;

public class CreateJournalVoucherValidator : AbstractValidator<CreateJournalVoucherCommand>
{
    public CreateJournalVoucherValidator()
    {
        RuleFor(x => x.Dto.Lines).NotEmpty().WithMessage("Journal voucher must have at least one line");
        RuleForEach(x => x.Dto.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.LedgerId).GreaterThan(0);
            line.RuleFor(l => l).Must(l => l.DebitAmount > 0 || l.CreditAmount > 0)
                .WithMessage("Each line must have either debit or credit amount");
        });
    }
}

public class CreateJournalVoucherHandler : IRequestHandler<CreateJournalVoucherCommand, ApiResponse<JournalVoucherDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateJournalVoucherHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<JournalVoucherDto>> Handle(CreateJournalVoucherCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var totalDebit = dto.Lines.Sum(l => l.DebitAmount);
        var totalCredit = dto.Lines.Sum(l => l.CreditAmount);

        var (isValid, error) = VoucherValidator.ValidateBalance(totalDebit, totalCredit);
        if (!isValid) return ApiResponse<JournalVoucherDto>.Failure(error!);

        var ledgerIds = dto.Lines.Select(l => l.LedgerId).Distinct().ToList();
        var existingCount = await _db.Ledgers.CountAsync(l => ledgerIds.Contains(l.Id), ct);
        if (existingCount != ledgerIds.Count)
            return ApiResponse<JournalVoucherDto>.Failure("One or more ledger IDs are invalid");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("JV-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Narration,
            TotalDebit = totalDebit,
            TotalCredit = totalCredit,
            VoucherTypeId = 3 // Journal
        };

        int lineNum = 1;
        foreach (var line in dto.Lines)
        {
            voucher.Details.Add(new VoucherDetail
            {
                LineNumber = lineNum++,
                LedgerId = line.LedgerId,
                DebitAmount = line.DebitAmount,
                CreditAmount = line.CreditAmount,
                CostCenterId = line.CostCenterId
            });
        }

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.Vouchers.Include(v => v.Details).ThenInclude(d => d.Ledger).FirstAsync(v => v.Id == voucher.Id, ct);
        var result = new JournalVoucherDto
        {
            Id = saved.Id,
            VoucherNo = saved.VoucherNumber,
            Date = saved.VoucherDate,
            Narration = saved.CommonNarration,
            TotalDebit = saved.TotalDebit,
            TotalCredit = saved.TotalCredit,
            Lines = saved.Details.Select(d => new JournalLineDto
            {
                LedgerId = d.LedgerId,
                LedgerName = d.Ledger?.Name,
                DebitAmount = d.DebitAmount,
                CreditAmount = d.CreditAmount,
                CostCenterId = d.CostCenterId
            }).ToList()
        };

        return ApiResponse<JournalVoucherDto>.Success(result, "Journal voucher created");
    }
}

// ── Contra Voucher ───────────────────────────────────────────────────

public record CreateContraVoucherCommand(CreateContraVoucherDto Dto) : IRequest<ApiResponse<ContraVoucherDto>>;

public class CreateContraVoucherValidator : AbstractValidator<CreateContraVoucherCommand>
{
    public CreateContraVoucherValidator()
    {
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.FromBankLedgerId).GreaterThan(0);
        RuleFor(x => x.Dto.ToBankLedgerId).GreaterThan(0);
        RuleFor(x => x.Dto).Must(d => d.FromBankLedgerId != d.ToBankLedgerId)
            .WithMessage("From and To bank ledgers must be different");
    }
}

public class CreateContraVoucherHandler : IRequestHandler<CreateContraVoucherCommand, ApiResponse<ContraVoucherDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateContraVoucherHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ContraVoucherDto>> Handle(CreateContraVoucherCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var fromLedger = await _db.Ledgers.FindAsync(new object[] { dto.FromBankLedgerId }, ct);
        if (fromLedger is null) return ApiResponse<ContraVoucherDto>.Failure("From bank ledger not found");

        var toLedger = await _db.Ledgers.FindAsync(new object[] { dto.ToBankLedgerId }, ct);
        if (toLedger is null) return ApiResponse<ContraVoucherDto>.Failure("To bank ledger not found");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("CV-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Narration,
            TotalDebit = dto.Amount,
            TotalCredit = dto.Amount,
            VoucherTypeId = 4 // Contra
        };

        // Debit ToBank, Credit FromBank
        voucher.Details.Add(new VoucherDetail { LineNumber = 1, LedgerId = dto.ToBankLedgerId, DebitAmount = dto.Amount, CreditAmount = 0, Narration = dto.Narration });
        voucher.Details.Add(new VoucherDetail { LineNumber = 2, LedgerId = dto.FromBankLedgerId, DebitAmount = 0, CreditAmount = dto.Amount, Narration = dto.Narration });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var result = new ContraVoucherDto
        {
            Id = voucher.Id,
            VoucherNo = voucher.VoucherNumber,
            Date = voucher.VoucherDate,
            FromBankLedgerId = dto.FromBankLedgerId,
            FromBankLedgerName = fromLedger.Name,
            ToBankLedgerId = dto.ToBankLedgerId,
            ToBankLedgerName = toLedger.Name,
            Amount = dto.Amount,
            Narration = voucher.CommonNarration
        };

        return ApiResponse<ContraVoucherDto>.Success(result, "Contra voucher created");
    }
}

// ── Debit Note ───────────────────────────────────────────────────────

public record CreateDebitNoteCommand(CreateDebitNoteDto Dto) : IRequest<ApiResponse<DebitNoteDto>>;

public class CreateDebitNoteValidator : AbstractValidator<CreateDebitNoteCommand>
{
    public CreateDebitNoteValidator()
    {
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.PartyLedgerId).GreaterThan(0);
    }
}

public class CreateDebitNoteHandler : IRequestHandler<CreateDebitNoteCommand, ApiResponse<DebitNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateDebitNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<DebitNoteDto>> Handle(CreateDebitNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var partyLedger = await _db.Ledgers.FindAsync(new object[] { dto.PartyLedgerId }, ct);
        if (partyLedger is null) return ApiResponse<DebitNoteDto>.Failure("Party ledger not found");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("DN-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Reason,
            ReferenceNumber = dto.ReferenceInvoiceNo,
            TotalDebit = dto.Amount,
            TotalCredit = dto.Amount,
            VoucherTypeId = 5 // Debit Note
        };

        voucher.Details.Add(new VoucherDetail { LineNumber = 1, LedgerId = dto.PartyLedgerId, DebitAmount = dto.Amount, CreditAmount = 0, Narration = dto.Reason });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var result = new DebitNoteDto
        {
            Id = voucher.Id,
            VoucherNo = voucher.VoucherNumber,
            Date = voucher.VoucherDate,
            PartyLedgerId = dto.PartyLedgerId,
            PartyLedgerName = partyLedger.Name,
            Amount = dto.Amount,
            Reason = dto.Reason,
            ReferenceInvoiceNo = dto.ReferenceInvoiceNo
        };

        return ApiResponse<DebitNoteDto>.Success(result, "Debit note created");
    }
}

// ── Credit Note ──────────────────────────────────────────────────────

public record CreateCreditNoteCommand(CreateCreditNoteDto Dto) : IRequest<ApiResponse<CreditNoteDto>>;

public class CreateCreditNoteValidator : AbstractValidator<CreateCreditNoteCommand>
{
    public CreateCreditNoteValidator()
    {
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.PartyLedgerId).GreaterThan(0);
    }
}

public class CreateCreditNoteHandler : IRequestHandler<CreateCreditNoteCommand, ApiResponse<CreditNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateCreditNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<CreditNoteDto>> Handle(CreateCreditNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var partyLedger = await _db.Ledgers.FindAsync(new object[] { dto.PartyLedgerId }, ct);
        if (partyLedger is null) return ApiResponse<CreditNoteDto>.Failure("Party ledger not found");

        var voucher = new Voucher
        {
            VoucherNumber = VoucherValidator.GenerateVoucherNumber("CN-", 1),
            VoucherDate = dto.Date,
            CommonNarration = dto.Reason,
            ReferenceNumber = dto.ReferenceInvoiceNo,
            TotalDebit = dto.Amount,
            TotalCredit = dto.Amount,
            VoucherTypeId = 6 // Credit Note
        };

        voucher.Details.Add(new VoucherDetail { LineNumber = 1, LedgerId = dto.PartyLedgerId, DebitAmount = 0, CreditAmount = dto.Amount, Narration = dto.Reason });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync(ct);

        var result = new CreditNoteDto
        {
            Id = voucher.Id,
            VoucherNo = voucher.VoucherNumber,
            Date = voucher.VoucherDate,
            PartyLedgerId = dto.PartyLedgerId,
            PartyLedgerName = partyLedger.Name,
            Amount = dto.Amount,
            Reason = dto.Reason,
            ReferenceInvoiceNo = dto.ReferenceInvoiceNo
        };

        return ApiResponse<CreditNoteDto>.Success(result, "Credit note created");
    }
}
