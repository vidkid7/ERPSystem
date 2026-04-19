using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Queries;

namespace UltimateERP.API.Controllers.Reporting;

[ApiController]
[Route("api/reports/financial")]
[Authorize]
public class FinancialReportController : ControllerBase
{
    private readonly IMediator _mediator;
    public FinancialReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet("balance-sheet")]
    public async Task<ActionResult<ApiResponse<BalanceSheetReportDto>>> GetBalanceSheet(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetBalanceSheetQuery(fromDate, toDate)));

    [HttpGet("profit-loss")]
    public async Task<ActionResult<ApiResponse<ProfitLossReportDto>>> GetProfitLoss(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetProfitLossQuery(fromDate, toDate)));

    [HttpGet("cash-flow")]
    public async Task<ActionResult<ApiResponse<CashFlowDto>>> GetCashFlow(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetCashFlowQuery(fromDate, toDate)));

    [HttpGet("vat")]
    public async Task<ActionResult<ApiResponse<VATReportDto>>> GetVATReport(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetVATReportQuery(fromDate, toDate)));

    [HttpGet("tds")]
    public async Task<ActionResult<ApiResponse<TDSReportDto>>> GetTDSReport(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetTDSReportQuery(fromDate, toDate)));
}
