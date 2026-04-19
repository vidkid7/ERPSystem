using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.Commands;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Queries;

namespace UltimateERP.API.Controllers.Account;

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class YearEndController : ControllerBase
{
    private readonly IMediator _mediator;
    public YearEndController(IMediator mediator) => _mediator = mediator;

    [HttpGet("fiscal-years")]
    public async Task<ActionResult<ApiResponse<List<FiscalYearDto>>>> GetFiscalYears()
        => Ok(await _mediator.Send(new GetFiscalYearsQuery()));

    [HttpGet("closing-trial-balance/{fiscalYearId}")]
    public async Task<ActionResult<ApiResponse<List<ClosingTrialBalanceDto>>>> GetClosingTrialBalance(int fiscalYearId)
        => Ok(await _mediator.Send(new GetClosingTrialBalanceQuery(fiscalYearId)));

    [HttpPost("close/{fiscalYearId}")]
    public async Task<ActionResult<ApiResponse<YearEndClosingResultDto>>> PerformClosing(int fiscalYearId)
    {
        var result = await _mediator.Send(new PerformYearEndClosingCommand(fiscalYearId));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("lock/{fiscalYearId}")]
    public async Task<ActionResult<ApiResponse<bool>>> LockFiscalYear(int fiscalYearId)
    {
        var result = await _mediator.Send(new LockFiscalYearCommand(fiscalYearId));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
