using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.Commands;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Queries;

namespace UltimateERP.API.Controllers.Account;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccountMasterController(IMediator mediator) => _mediator = mediator;

    // ── PaymentTerm ──────────────────────────────────────────────────

    [HttpGet("payment-terms")]
    public async Task<ActionResult<ApiResponse<List<PaymentTermDto>>>> GetPaymentTerms(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPaymentTermsQuery(search, page, pageSize)));

    [HttpGet("payment-terms/{id}")]
    public async Task<ActionResult<ApiResponse<PaymentTermDto>>> GetPaymentTerm(int id)
    {
        var result = await _mediator.Send(new GetPaymentTermByIdQuery(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("payment-terms")]
    public async Task<ActionResult<ApiResponse<PaymentTermDto>>> CreatePaymentTerm([FromBody] CreatePaymentTermDto dto)
    {
        var result = await _mediator.Send(new CreatePaymentTermCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("payment-terms")]
    public async Task<ActionResult<ApiResponse<PaymentTermDto>>> UpdatePaymentTerm([FromBody] UpdatePaymentTermDto dto)
    {
        var result = await _mediator.Send(new UpdatePaymentTermCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("payment-terms/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePaymentTerm(int id)
    {
        var result = await _mediator.Send(new DeletePaymentTermCommand(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    // ── PaymentMode ──────────────────────────────────────────────────

    [HttpGet("payment-modes")]
    public async Task<ActionResult<ApiResponse<List<PaymentModeDto>>>> GetPaymentModes(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPaymentModesQuery(search, page, pageSize)));

    [HttpGet("payment-modes/{id}")]
    public async Task<ActionResult<ApiResponse<PaymentModeDto>>> GetPaymentMode(int id)
    {
        var result = await _mediator.Send(new GetPaymentModeByIdQuery(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("payment-modes")]
    public async Task<ActionResult<ApiResponse<PaymentModeDto>>> CreatePaymentMode([FromBody] CreatePaymentModeDto dto)
    {
        var result = await _mediator.Send(new CreatePaymentModeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("payment-modes")]
    public async Task<ActionResult<ApiResponse<PaymentModeDto>>> UpdatePaymentMode([FromBody] UpdatePaymentModeDto dto)
    {
        var result = await _mediator.Send(new UpdatePaymentModeCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("payment-modes/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePaymentMode(int id)
    {
        var result = await _mediator.Send(new DeletePaymentModeCommand(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
