using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HR.Commands;
using UltimateERP.Application.Features.HR.DTOs;
using UltimateERP.Application.Features.HR.Queries;

namespace UltimateERP.API.Controllers.HR;

[ApiController]
[Route("api/hr/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;
    public EmployeeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? branchId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetEmployeesQuery(search, branchId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> Create([FromBody] CreateEmployeeDto dto)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/hr/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    public AttendanceController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AttendanceDto>>>> GetAll(
        [FromQuery] int? employeeId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _mediator.Send(new GetAttendanceQuery(employeeId, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AttendanceDto>>> Create([FromBody] CreateAttendanceDto dto)
    {
        var result = await _mediator.Send(new CreateAttendanceCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/hr/[controller]")]
[Authorize]
public class LeaveController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LeaveDto>>>> GetAll(
        [FromQuery] int? employeeId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetLeavesQuery(employeeId, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LeaveDto>>> Create([FromBody] CreateLeaveDto dto)
    {
        var result = await _mediator.Send(new CreateLeaveCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id}/approve")]
    public async Task<ActionResult<ApiResponse<LeaveDto>>> Approve(int id, [FromQuery] int approvedBy, [FromQuery] bool approve = true)
    {
        var result = await _mediator.Send(new ApproveLeaveCommand(id, approvedBy, approve));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/hr/[controller]")]
[Authorize]
public class ExpenseClaimController : ControllerBase
{
    private readonly IMediator _mediator;
    public ExpenseClaimController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ExpenseClaimDto>>> Create([FromBody] CreateExpenseClaimDto dto)
    {
        var result = await _mediator.Send(new CreateExpenseClaimCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
