using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Queries;

namespace UltimateERP.API.Controllers.Reporting;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet("trial-balance")]
    public async Task<ActionResult<ApiResponse<TrialBalanceDto>>> GetTrialBalance([FromQuery] DateTime? asOfDate)
        => Ok(await _mediator.Send(new GetTrialBalanceQuery(asOfDate)));

    [HttpGet("day-book")]
    public async Task<ActionResult<ApiResponse<List<DayBookEntryDto>>>> GetDayBook([FromQuery] DateTime date)
        => Ok(await _mediator.Send(new GetDayBookQuery(date)));

    [HttpGet("ledger-statement/{ledgerId}")]
    public async Task<ActionResult<ApiResponse<List<LedgerStatementLineDto>>>> GetLedgerStatement(
        int ledgerId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetLedgerStatementQuery(ledgerId, fromDate, toDate)));
}

[ApiController]
[Route("api/posting")]
[Authorize]
public class PostingController : ControllerBase
{
    private readonly IMediator _mediator;
    public PostingController(IMediator mediator) => _mediator = mediator;

    [HttpPost("purchase/{invoiceId}")]
    public async Task<ActionResult<ApiResponse<int>>> PostPurchaseInvoice(int invoiceId, [FromQuery] int purchaseLedgerId)
    {
        var result = await _mediator.Send(new PostPurchaseInvoiceCommand(invoiceId, purchaseLedgerId));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("sales/{invoiceId}")]
    public async Task<ActionResult<ApiResponse<int>>> PostSalesInvoice(int invoiceId, [FromQuery] int salesLedgerId)
    {
        var result = await _mediator.Send(new PostSalesInvoiceCommand(invoiceId, salesLedgerId));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
