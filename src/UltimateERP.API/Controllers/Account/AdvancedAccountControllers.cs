using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.Commands;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Queries;
using UltimateERP.Domain.Enums;

namespace UltimateERP.API.Controllers.Account;

// ── PDC Controller ────────────────────────────────────────────────────

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class PDCController : ControllerBase
{
    private readonly IMediator _mediator;
    public PDCController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PDCDto>>>> GetAll(
        [FromQuery] PDCStatus? status, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPDCsQuery(status, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PDCDto>>> Create([FromBody] CreatePDCDto dto)
    {
        var result = await _mediator.Send(new CreatePDCCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<ApiResponse<PDCDto>>> UpdateStatus(int id, [FromBody] PDCStatus status)
    {
        var result = await _mediator.Send(new UpdatePDCStatusCommand(id, status));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── ODC Controller ────────────────────────────────────────────────────

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class ODCController : ControllerBase
{
    private readonly IMediator _mediator;
    public ODCController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ODCDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetODCsQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ODCDto>>> Create([FromBody] CreateODCDto dto)
    {
        var result = await _mediator.Send(new CreateODCCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<ApiResponse<ODCDto>>> UpdateStatus(int id, [FromBody] string status)
    {
        var result = await _mediator.Send(new UpdateODCStatusCommand(id, status));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── Bank Guarantee Controller ─────────────────────────────────────────

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class BankGuaranteeController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankGuaranteeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BankGuaranteeDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetBankGuaranteesQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BankGuaranteeDto>>> Create([FromBody] CreateBankGuaranteeDto dto)
    {
        var result = await _mediator.Send(new CreateBankGuaranteeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Letter of Credit Controller ───────────────────────────────────────

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class LetterOfCreditController : ControllerBase
{
    private readonly IMediator _mediator;
    public LetterOfCreditController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LetterOfCreditDto>>>> GetAll(
        [FromQuery] LCStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetLettersOfCreditQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LetterOfCreditDto>>> Create([FromBody] CreateLetterOfCreditDto dto)
    {
        var result = await _mediator.Send(new CreateLetterOfCreditCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Bank Reconciliation Controller ────────────────────────────────────

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class BankReconciliationController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankReconciliationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("unreconciled")]
    public async Task<ActionResult<ApiResponse<List<BankReconciliationDto>>>> GetUnreconciled(
        [FromQuery] int ledgerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _mediator.Send(new GetUnreconciledTransactionsQuery(ledgerId, page, pageSize)));

    [HttpPost("reconcile")]
    public async Task<ActionResult<ApiResponse<BankReconciliationDto>>> Reconcile([FromBody] ReconcileTransactionDto dto)
    {
        var result = await _mediator.Send(new ReconcileTransactionCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
