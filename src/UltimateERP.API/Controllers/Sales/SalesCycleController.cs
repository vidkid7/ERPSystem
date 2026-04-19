using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Sales.Commands;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Features.Sales.Queries;

namespace UltimateERP.API.Controllers.Sales;

// ── Sales Quotation ─────────────────────────────────────────────────

[ApiController]
[Route("api/sales/quotations")]
[Authorize]
public class SalesQuotationController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesQuotationController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesQuotationDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSalesQuotationsQuery(customerId, fromDate, toDate, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesQuotationDto>>> Create([FromBody] CreateSalesQuotationDto dto)
    {
        var result = await _mediator.Send(new CreateSalesQuotationCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<SalesQuotationDto>>> Approve(int id)
    {
        var result = await _mediator.Send(new ApproveSalesQuotationCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── Sales Order ─────────────────────────────────────────────────────

[ApiController]
[Route("api/sales/orders")]
[Authorize]
public class SalesOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesOrderController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesOrderDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSalesOrdersQuery(customerId, fromDate, toDate, status, page, pageSize)));

    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<List<SalesOrderDto>>>> GetPending([FromQuery] int? customerId)
        => Ok(await _mediator.Send(new GetPendingSalesOrdersQuery(customerId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Create([FromBody] CreateSalesOrderDto dto)
    {
        var result = await _mediator.Send(new CreateSalesOrderCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Approve(int id)
    {
        var result = await _mediator.Send(new ApproveSalesOrderCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Cancel(int id)
    {
        var result = await _mediator.Send(new CancelSalesOrderCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── Sales Allotment ─────────────────────────────────────────────────

[ApiController]
[Route("api/sales/allotments")]
[Authorize]
public class SalesAllotmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesAllotmentController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesAllotmentDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSalesAllotmentsQuery(customerId, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesAllotmentDto>>> Create([FromBody] CreateSalesAllotmentDto dto)
    {
        var result = await _mediator.Send(new CreateSalesAllotmentCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Sales Delivery Note ─────────────────────────────────────────────

[ApiController]
[Route("api/sales/deliveries")]
[Authorize]
public class SalesDeliveryNoteController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesDeliveryNoteController(IMediator mediator) => _mediator = mediator;

    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<List<SalesDeliveryNoteDto>>>> GetPending([FromQuery] int? customerId)
        => Ok(await _mediator.Send(new GetPendingDeliveriesQuery(customerId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesDeliveryNoteDto>>> Create([FromBody] CreateSalesDeliveryNoteDto dto)
    {
        var result = await _mediator.Send(new CreateSalesDeliveryNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Sales Return ────────────────────────────────────────────────────

[ApiController]
[Route("api/sales/returns")]
[Authorize]
public class SalesReturnController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesReturnController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesReturnDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSalesReturnsQuery(customerId, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesReturnDto>>> Create([FromBody] CreateSalesReturnDto dto)
    {
        var result = await _mediator.Send(new CreateSalesReturnCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Sales Debit Note ────────────────────────────────────────────────

[ApiController]
[Route("api/sales/debit-notes")]
[Authorize]
public class SalesDebitNoteController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesDebitNoteController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesDebitNoteDto>>> Create([FromBody] CreateSalesDebitNoteDto dto)
    {
        var result = await _mediator.Send(new CreateSalesDebitNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Sales Credit Note ───────────────────────────────────────────────

[ApiController]
[Route("api/sales/credit-notes")]
[Authorize]
public class SalesCreditNoteController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesCreditNoteController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesCreditNoteDto>>> Create([FromBody] CreateSalesCreditNoteDto dto)
    {
        var result = await _mediator.Send(new CreateSalesCreditNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
