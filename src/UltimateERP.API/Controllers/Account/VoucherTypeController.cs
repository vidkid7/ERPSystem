using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.Commands;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Queries;

namespace UltimateERP.API.Controllers.Account;

[ApiController]
[Route("api/account/voucher")]
[Authorize]
public class VoucherTypeController : ControllerBase
{
    private readonly IMediator _mediator;
    public VoucherTypeController(IMediator mediator) => _mediator = mediator;

    // ── Receipt Voucher ──────────────────────────────────────────────

    [HttpPost("receipt")]
    public async Task<ActionResult<ApiResponse<ReceiptVoucherDto>>> CreateReceipt([FromBody] CreateReceiptVoucherDto dto)
    {
        var result = await _mediator.Send(new CreateReceiptVoucherCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("receipts")]
    public async Task<ActionResult<ApiResponse<List<ReceiptVoucherDto>>>> GetReceipts(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int? partyLedgerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetReceiptVouchersQuery(fromDate, toDate, partyLedgerId, page, pageSize)));

    // ── Payment Voucher ──────────────────────────────────────────────

    [HttpPost("payment")]
    public async Task<ActionResult<ApiResponse<PaymentVoucherDto>>> CreatePayment([FromBody] CreatePaymentVoucherDto dto)
    {
        var result = await _mediator.Send(new CreatePaymentVoucherCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("payments")]
    public async Task<ActionResult<ApiResponse<List<PaymentVoucherDto>>>> GetPayments(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int? partyLedgerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPaymentVouchersQuery(fromDate, toDate, partyLedgerId, page, pageSize)));

    // ── Journal Voucher ──────────────────────────────────────────────

    [HttpPost("journal")]
    public async Task<ActionResult<ApiResponse<JournalVoucherDto>>> CreateJournal([FromBody] CreateJournalVoucherDto dto)
    {
        var result = await _mediator.Send(new CreateJournalVoucherCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("journals")]
    public async Task<ActionResult<ApiResponse<List<JournalVoucherDto>>>> GetJournals(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetJournalVouchersQuery(fromDate, toDate, page, pageSize)));

    // ── Contra Voucher ───────────────────────────────────────────────

    [HttpPost("contra")]
    public async Task<ActionResult<ApiResponse<ContraVoucherDto>>> CreateContra([FromBody] CreateContraVoucherDto dto)
    {
        var result = await _mediator.Send(new CreateContraVoucherCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("contras")]
    public async Task<ActionResult<ApiResponse<List<ContraVoucherDto>>>> GetContras(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetContraVouchersQuery(fromDate, toDate, page, pageSize)));

    // ── Debit Note ───────────────────────────────────────────────────

    [HttpPost("debit-note")]
    public async Task<ActionResult<ApiResponse<DebitNoteDto>>> CreateDebitNote([FromBody] CreateDebitNoteDto dto)
    {
        var result = await _mediator.Send(new CreateDebitNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    // ── Credit Note ──────────────────────────────────────────────────

    [HttpPost("credit-note")]
    public async Task<ActionResult<ApiResponse<CreditNoteDto>>> CreateCreditNote([FromBody] CreateCreditNoteDto dto)
    {
        var result = await _mediator.Send(new CreateCreditNoteCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
