using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Finance.Commands;
using UltimateERP.Application.Features.Finance.DTOs;
using UltimateERP.Application.Features.Finance.Queries;

namespace UltimateERP.API.Controllers.Finance;

[ApiController]
[Route("api/finance/[controller]")]
[Authorize]
public class LoanController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoanController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LoanDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetLoansQuery(search, status, page, pageSize)));

    [HttpGet("{loanId}/emis")]
    public async Task<ActionResult<ApiResponse<List<LoanEMIDto>>>> GetEMISchedule(int loanId)
        => Ok(await _mediator.Send(new GetEMIScheduleQuery(loanId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LoanDto>>> Create([FromBody] CreateLoanDto dto)
    {
        var result = await _mediator.Send(new CreateLoanCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("emi/process")]
    public async Task<ActionResult<ApiResponse<LoanEMIDto>>> ProcessEMI([FromBody] ProcessEMIDto dto)
    {
        var result = await _mediator.Send(new ProcessEMICommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
