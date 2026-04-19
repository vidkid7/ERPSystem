using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Queries;

namespace UltimateERP.API.Controllers.Reporting;

[ApiController]
[Route("api/reports/inventory")]
[Authorize]
public class InventoryReportController : ControllerBase
{
    private readonly IMediator _mediator;
    public InventoryReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet("stock-aging")]
    public async Task<ActionResult<ApiResponse<List<StockAgingDto>>>> GetStockAging()
        => Ok(await _mediator.Send(new GetStockAgingQuery()));

    [HttpGet("stock-movement")]
    public async Task<ActionResult<ApiResponse<List<StockMovementDto>>>> GetStockMovement(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetStockMovementQuery(fromDate, toDate)));

    [HttpGet("sales-analysis")]
    public async Task<ActionResult<ApiResponse<List<SalesAnalysisDto>>>> GetSalesAnalysis(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? groupBy)
        => Ok(await _mediator.Send(new GetSalesAnalysisQuery(fromDate, toDate, groupBy)));

    [HttpGet("purchase-analysis")]
    public async Task<ActionResult<ApiResponse<List<PurchaseAnalysisDto>>>> GetPurchaseAnalysis(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        => Ok(await _mediator.Send(new GetPurchaseAnalysisQuery(fromDate, toDate)));
}
