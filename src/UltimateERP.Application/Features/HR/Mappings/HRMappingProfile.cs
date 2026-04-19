using AutoMapper;
using UltimateERP.Application.Features.HR.DTOs;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.HR.Mappings;

public class HRMappingProfile : Profile
{
    public HRMappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch != null ? s.Branch.Name : null));

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s =>
                s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<Leave, LeaveDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s =>
                s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<ExpenseClaim, ExpenseClaimDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s =>
                s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
