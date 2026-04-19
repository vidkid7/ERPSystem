using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Finance.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Finance;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Finance.Commands;

// Create Loan
public record CreateLoanCommand(CreateLoanDto Loan) : IRequest<ApiResponse<LoanDto>>;

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.Loan.LoanNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Loan.PrincipalAmount).GreaterThan(0);
        RuleFor(x => x.Loan.InterestRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Loan.TenureMonths).GreaterThan(0);
    }
}

public class CreateLoanHandler : IRequestHandler<CreateLoanCommand, ApiResponse<LoanDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateLoanHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LoanDto>> Handle(CreateLoanCommand request, CancellationToken ct)
    {
        var dto = request.Loan;
        var exists = await _db.Loans.AnyAsync(l => l.LoanNumber == dto.LoanNumber, ct);
        if (exists) return ApiResponse<LoanDto>.Failure($"Loan number {dto.LoanNumber} already exists");

        var monthlyRate = dto.InterestRate / 12 / 100;
        var emi = monthlyRate > 0
            ? dto.PrincipalAmount * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), dto.TenureMonths)
              / ((decimal)Math.Pow((double)(1 + monthlyRate), dto.TenureMonths) - 1)
            : dto.PrincipalAmount / dto.TenureMonths;

        var loan = new Loan
        {
            LoanNumber = dto.LoanNumber,
            LoanDate = dto.LoanDate,
            BorrowerName = dto.BorrowerName,
            BorrowerContact = dto.BorrowerContact,
            PrincipalAmount = dto.PrincipalAmount,
            InterestRate = dto.InterestRate,
            TenureMonths = dto.TenureMonths,
            DisbursementDate = dto.DisbursementDate,
            EMIAmount = Math.Round(emi, 2),
            Status = LoanStatus.Active,
            VehicleDetailId = dto.VehicleDetailId
        };

        _db.Loans.Add(loan);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LoanDto>.Success(_mapper.Map<LoanDto>(loan), "Loan created");
    }
}

// Process EMI Payment
public record ProcessEMICommand(ProcessEMIDto EMI) : IRequest<ApiResponse<LoanEMIDto>>;

public class ProcessEMIHandler : IRequestHandler<ProcessEMICommand, ApiResponse<LoanEMIDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ProcessEMIHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LoanEMIDto>> Handle(ProcessEMICommand request, CancellationToken ct)
    {
        var emi = await _db.LoanEMIs.Include(e => e.Loan).FirstOrDefaultAsync(e => e.Id == request.EMI.LoanEMIId, ct);
        if (emi is null) return ApiResponse<LoanEMIDto>.Failure("EMI record not found");

        emi.PaidAmount = request.EMI.PaidAmount;
        emi.PaidDate = request.EMI.PaidDate;
        emi.Status = EMIStatus.Paid;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<LoanEMIDto>.Success(_mapper.Map<LoanEMIDto>(emi), "EMI payment processed");
    }
}

// Close Loan
public record CloseLoanCommand(CloseLoanDto Dto) : IRequest<ApiResponse<LoanDto>>;

public class CloseLoanHandler : IRequestHandler<CloseLoanCommand, ApiResponse<LoanDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CloseLoanHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LoanDto>> Handle(CloseLoanCommand request, CancellationToken ct)
    {
        var loan = await _db.Loans.Include(l => l.EMIs)
            .FirstOrDefaultAsync(l => l.Id == request.Dto.LoanId, ct);
        if (loan is null) return ApiResponse<LoanDto>.Failure("Loan not found");

        var unpaid = loan.EMIs.Any(e => e.Status != EMIStatus.Paid);
        if (unpaid) return ApiResponse<LoanDto>.Failure("All EMIs must be paid before closing the loan");

        loan.Status = LoanStatus.Closed;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LoanDto>.Success(_mapper.Map<LoanDto>(loan), "Loan closed successfully");
    }
}

// Apply Rebate
public record ApplyRebateCommand(ApplyAdjustmentDto Dto) : IRequest<ApiResponse<LoanEMIDto>>;

public class ApplyRebateHandler : IRequestHandler<ApplyRebateCommand, ApiResponse<LoanEMIDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApplyRebateHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LoanEMIDto>> Handle(ApplyRebateCommand request, CancellationToken ct)
    {
        var emi = await _db.LoanEMIs.Include(e => e.Loan)
            .FirstOrDefaultAsync(e => e.Id == request.Dto.LoanEMIId, ct);
        if (emi is null) return ApiResponse<LoanEMIDto>.Failure("EMI record not found");

        emi.PaidAmount = Math.Max(0, emi.PaidAmount - request.Dto.Amount);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LoanEMIDto>.Success(_mapper.Map<LoanEMIDto>(emi), "Rebate applied successfully");
    }
}

// Apply Penalty
public record ApplyPenaltyCommand(ApplyAdjustmentDto Dto) : IRequest<ApiResponse<LoanEMIDto>>;

public class ApplyPenaltyHandler : IRequestHandler<ApplyPenaltyCommand, ApiResponse<LoanEMIDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApplyPenaltyHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LoanEMIDto>> Handle(ApplyPenaltyCommand request, CancellationToken ct)
    {
        var emi = await _db.LoanEMIs.Include(e => e.Loan)
            .FirstOrDefaultAsync(e => e.Id == request.Dto.LoanEMIId, ct);
        if (emi is null) return ApiResponse<LoanEMIDto>.Failure("EMI record not found");

        emi.EMIAmount += request.Dto.Amount;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LoanEMIDto>.Success(_mapper.Map<LoanEMIDto>(emi), "Penalty applied successfully");
    }
}
