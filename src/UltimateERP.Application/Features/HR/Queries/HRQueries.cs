using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HR.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.HR.Queries;

// Employees
public record GetEmployeesQuery(string? Search, int? BranchId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<EmployeeDto>>>;

public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, ApiResponse<List<EmployeeDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetEmployeesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken ct)
    {
        var query = _db.Employees.Include(e => e.Branch).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(s)
                                  || e.EmployeeCode.ToLower().Contains(s));
        }
        if (request.BranchId.HasValue)
            query = query.Where(e => e.BranchId == request.BranchId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.EmployeeCode)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<EmployeeDto>>.Success(items, "Employees retrieved", total);
    }
}

// Attendance
public record GetAttendanceQuery(int? EmployeeId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 50)
    : IRequest<ApiResponse<List<AttendanceDto>>>;

public class GetAttendanceHandler : IRequestHandler<GetAttendanceQuery, ApiResponse<List<AttendanceDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetAttendanceHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<AttendanceDto>>> Handle(GetAttendanceQuery request, CancellationToken ct)
    {
        var query = _db.Attendances.Include(a => a.Employee).AsQueryable();

        if (request.EmployeeId.HasValue) query = query.Where(a => a.EmployeeId == request.EmployeeId);
        if (request.FromDate.HasValue) query = query.Where(a => a.AttendanceDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(a => a.AttendanceDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AttendanceDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<AttendanceDto>>.Success(items, "Attendance records retrieved", total);
    }
}

// Leaves
public record GetLeavesQuery(int? EmployeeId, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LeaveDto>>>;

public class GetLeavesHandler : IRequestHandler<GetLeavesQuery, ApiResponse<List<LeaveDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetLeavesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LeaveDto>>> Handle(GetLeavesQuery request, CancellationToken ct)
    {
        var query = _db.Leaves.Include(l => l.Employee).AsQueryable();

        if (request.EmployeeId.HasValue) query = query.Where(l => l.EmployeeId == request.EmployeeId);
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.LeaveStatus>(request.Status, out var status))
            query = query.Where(l => l.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(l => l.FromDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<LeaveDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LeaveDto>>.Success(items, "Leaves retrieved", total);
    }
}
