using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HR.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.HR.Commands;

// Create Employee
public record CreateEmployeeCommand(CreateEmployeeDto Employee) : IRequest<ApiResponse<EmployeeDto>>;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.Employee.EmployeeCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Employee.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Employee.LastName).NotEmpty().MaximumLength(100);
    }
}

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, ApiResponse<EmployeeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateEmployeeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        var dto = request.Employee;
        var exists = await _db.Employees.AnyAsync(e => e.EmployeeCode == dto.EmployeeCode, ct);
        if (exists) return ApiResponse<EmployeeDto>.Failure($"Employee code {dto.EmployeeCode} already exists");

        var emp = new Employee
        {
            EmployeeCode = dto.EmployeeCode,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            MaritalStatus = dto.MaritalStatus,
            Address = dto.Address,
            City = dto.City,
            Phone = dto.Phone,
            Email = dto.Email,
            JoiningDate = dto.JoiningDate,
            DesignationId = dto.DesignationId,
            DepartmentId = dto.DepartmentId,
            BranchId = dto.BranchId,
            BankAccountNumber = dto.BankAccountNumber,
            PANNumber = dto.PANNumber
        };

        _db.Employees.Add(emp);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(emp), "Employee created");
    }
}

// Create Attendance
public record CreateAttendanceCommand(CreateAttendanceDto Attendance) : IRequest<ApiResponse<AttendanceDto>>;

public class CreateAttendanceHandler : IRequestHandler<CreateAttendanceCommand, ApiResponse<AttendanceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateAttendanceHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<AttendanceDto>> Handle(CreateAttendanceCommand request, CancellationToken ct)
    {
        var dto = request.Attendance;
        var hours = (dto.CheckInTime.HasValue && dto.CheckOutTime.HasValue)
            ? (decimal)(dto.CheckOutTime.Value - dto.CheckInTime.Value).TotalHours : 0;

        var attendance = new Attendance
        {
            EmployeeId = dto.EmployeeId,
            AttendanceDate = dto.AttendanceDate,
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime,
            WorkingHours = hours,
            Status = hours > 0 ? AttendanceStatus.Present : AttendanceStatus.Absent,
            Remarks = dto.Remarks
        };

        _db.Attendances.Add(attendance);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<AttendanceDto>.Success(_mapper.Map<AttendanceDto>(attendance), "Attendance recorded");
    }
}

// Create Leave
public record CreateLeaveCommand(CreateLeaveDto Leave) : IRequest<ApiResponse<LeaveDto>>;

public class CreateLeaveHandler : IRequestHandler<CreateLeaveCommand, ApiResponse<LeaveDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateLeaveHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LeaveDto>> Handle(CreateLeaveCommand request, CancellationToken ct)
    {
        var dto = request.Leave;
        var totalDays = (decimal)(dto.ToDate - dto.FromDate).TotalDays + 1;

        var leave = new Leave
        {
            EmployeeId = dto.EmployeeId,
            LeaveTypeId = dto.LeaveTypeId,
            FromDate = dto.FromDate,
            ToDate = dto.ToDate,
            TotalDays = totalDays,
            Reason = dto.Reason,
            Status = LeaveStatus.Pending
        };

        _db.Leaves.Add(leave);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LeaveDto>.Success(_mapper.Map<LeaveDto>(leave), "Leave request created");
    }
}

// Approve Leave
public record ApproveLeaveCommand(int LeaveId, int ApprovedBy, bool Approve) : IRequest<ApiResponse<LeaveDto>>;

public class ApproveLeaveHandler : IRequestHandler<ApproveLeaveCommand, ApiResponse<LeaveDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApproveLeaveHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LeaveDto>> Handle(ApproveLeaveCommand request, CancellationToken ct)
    {
        var leave = await _db.Leaves.FindAsync(new object[] { request.LeaveId }, ct);
        if (leave is null) return ApiResponse<LeaveDto>.Failure("Leave request not found");

        leave.Status = request.Approve ? LeaveStatus.Approved : LeaveStatus.Rejected;
        leave.ApprovedBy = request.ApprovedBy;
        leave.ApprovedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<LeaveDto>.Success(_mapper.Map<LeaveDto>(leave),
            request.Approve ? "Leave approved" : "Leave rejected");
    }
}

// Create Expense Claim
public record CreateExpenseClaimCommand(CreateExpenseClaimDto Claim) : IRequest<ApiResponse<ExpenseClaimDto>>;

public class CreateExpenseClaimHandler : IRequestHandler<CreateExpenseClaimCommand, ApiResponse<ExpenseClaimDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateExpenseClaimHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ExpenseClaimDto>> Handle(CreateExpenseClaimCommand request, CancellationToken ct)
    {
        var dto = request.Claim;
        var claim = new ExpenseClaim
        {
            EmployeeId = dto.EmployeeId,
            ClaimDate = dto.ClaimDate,
            TotalAmount = dto.TotalAmount,
            Status = ExpenseClaimStatus.Pending
        };

        _db.ExpenseClaims.Add(claim);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ExpenseClaimDto>.Success(_mapper.Map<ExpenseClaimDto>(claim), "Expense claim created");
    }
}
