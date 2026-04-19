using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.Commands;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Features.Purchase.Queries;
using UltimateERP.Domain.Enums;

namespace UltimateERP.API.Controllers.Purchase;

[ApiController]
[Route("api/purchase")]
[Authorize]
public class PurchaseCycleController : ControllerBase
{
    private readonly IMediator _mediator;
    public PurchaseCycleController(IMediator mediator) => _mediator = mediator;

    // ── Purchase Quotation ───────────────────────────────────────────

    [HttpGet("quotations")]
    public async Task<ActionResult<ApiResponse<List<PurchaseQuotationDto>>>> GetQuotations(
        [FromQuery] int? vendorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] PurchaseDocumentStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPurchaseQuotationsQuery(vendorId, fromDate, toDate, status, page, pageSize)));

    [HttpPost("quotations")]
    public async Task<ActionResult<ApiResponse<PurchaseQuotationDto>>> CreateQuotation([FromBody] CreatePurchaseQuotationDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseQuotationCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("quotations/{id}/approve")]
    public async Task<ActionResult<ApiResponse<PurchaseQuotationDto>>> ApproveQuotation(int id)
    {
        var result = await _mediator.Send(new ApprovePurchaseQuotationCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // ── Purchase Order ───────────────────────────────────────────────

    [HttpGet("orders")]
    public async Task<ActionResult<ApiResponse<List<PurchaseOrderDto>>>> GetOrders(
        [FromQuery] int? vendorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] PurchaseDocumentStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPurchaseOrdersQuery(vendorId, fromDate, toDate, status, page, pageSize)));

    [HttpGet("orders/pending")]
    public async Task<ActionResult<ApiResponse<List<PurchaseOrderDto>>>> GetPendingOrders(
        [FromQuery] int? vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPendingPurchaseOrdersQuery(vendorId, page, pageSize)));

    [HttpPost("orders")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> CreateOrder([FromBody] CreatePurchaseOrderDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseOrderCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("orders/{id}/approve")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> ApproveOrder(int id)
    {
        var result = await _mediator.Send(new ApprovePurchaseOrderCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // ── Receipt Note (GRN) ──────────────────────────────────────────

    [HttpGet("receipt-notes")]
    public async Task<ActionResult<ApiResponse<List<ReceiptNoteDto>>>> GetReceiptNotes(
        [FromQuery] int? vendorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetReceiptNotesQuery(vendorId, fromDate, toDate, page, pageSize)));

    [HttpGet("receipt-notes/pending")]
    public async Task<ActionResult<ApiResponse<List<ReceiptNoteDto>>>> GetPendingReceipts(
        [FromQuery] int? vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPendingReceiptsQuery(vendorId, page, pageSize)));

    [HttpPost("receipt-notes")]
    public async Task<ActionResult<ApiResponse<ReceiptNoteDto>>> CreateReceiptNote([FromBody] CreateReceiptNoteDto dto)
    {
        var result = await _mediator.Send(new CreateReceiptNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    // ── Purchase Return ──────────────────────────────────────────────

    [HttpGet("returns")]
    public async Task<ActionResult<ApiResponse<List<PurchaseReturnDto>>>> GetReturns(
        [FromQuery] int? vendorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPurchaseReturnsQuery(vendorId, fromDate, toDate, page, pageSize)));

    [HttpPost("returns")]
    public async Task<ActionResult<ApiResponse<PurchaseReturnDto>>> CreateReturn([FromBody] CreatePurchaseReturnDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseReturnCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    // ── Purchase Debit Note ──────────────────────────────────────────

    [HttpPost("debit-notes")]
    public async Task<ActionResult<ApiResponse<PurchaseDebitNoteDto>>> CreateDebitNote([FromBody] CreatePurchaseDebitNoteDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseDebitNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    // ── Purchase Credit Note ─────────────────────────────────────────

    [HttpPost("credit-notes")]
    public async Task<ActionResult<ApiResponse<PurchaseCreditNoteDto>>> CreateCreditNote([FromBody] CreatePurchaseCreditNoteDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseCreditNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
