using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Queries;

namespace UltimateERP.API.Controllers.Reporting;

[ApiController]
[Route("api/reports/additional")]
[Authorize]
public class AdditionalReportController : ControllerBase
{
    private readonly IMediator _mediator;
    public AdditionalReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet("cancel-day-book")]
    public async Task<ActionResult<ApiResponse<List<CancelDayBookDto>>>> GetCancelDayBook(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetCancelDayBookQuery(fromDate, toDate)));

    [HttpGet("cost-center-analysis")]
    public async Task<ActionResult<ApiResponse<List<CostCenterAnalysisDto>>>> GetCostCenterAnalysis(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetCostCenterAnalysisQuery(fromDate, toDate)));

    [HttpGet("bills-receivable")]
    public async Task<ActionResult<ApiResponse<List<BillsReceivableDto>>>> GetBillsReceivable(
        [FromQuery] DateTime? asOfDate)
        => Ok(await _mediator.Send(new GetBillsReceivableQuery(asOfDate)));

    [HttpGet("bills-payable")]
    public async Task<ActionResult<ApiResponse<List<BillsPayableDto>>>> GetBillsPayable(
        [FromQuery] DateTime? asOfDate)
        => Ok(await _mediator.Send(new GetBillsPayableQuery(asOfDate)));
}
