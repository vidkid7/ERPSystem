using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.Commands;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Features.Inventory.Queries;

namespace UltimateERP.API.Controllers.Inventory;

// ── Rack ────────────────────────────────────────────────────────────

[ApiController]
[Route("api/inventory/racks")]
[Authorize]
public class RackController : ControllerBase
{
    private readonly IMediator _mediator;
    public RackController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<RackDto>>>> GetAll([FromQuery] int? godownId)
        => Ok(await _mediator.Send(new GetRacksQuery(godownId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RackDto>>> Create([FromBody] CreateRackDto dto)
    {
        var result = await _mediator.Send(new CreateRackCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<RackDto>>> Update(int id, [FromBody] CreateRackDto dto)
    {
        var result = await _mediator.Send(new UpdateRackCommand(id, dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── Indent ──────────────────────────────────────────────────────────

[ApiController]
[Route("api/inventory/indents")]
[Authorize]
public class IndentController : ControllerBase
{
    private readonly IMediator _mediator;
    public IndentController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<IndentDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetIndentsQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IndentDto>>> Create([FromBody] CreateIndentDto dto)
    {
        var result = await _mediator.Send(new CreateIndentCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<IndentDto>>> Approve(int id)
    {
        var result = await _mediator.Send(new ApproveIndentCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── GatePass ────────────────────────────────────────────────────────

[ApiController]
[Route("api/inventory/gate-passes")]
[Authorize]
public class GatePassController : ControllerBase
{
    private readonly IMediator _mediator;
    public GatePassController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GatePassDto>>>> GetAll(
        [FromQuery] string? type, [FromQuery] bool? isApproved,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetGatePassesQuery(type, isApproved, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GatePassDto>>> Create([FromBody] CreateGatePassDto dto)
    {
        var result = await _mediator.Send(new CreateGatePassCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<GatePassDto>>> Approve(int id)
    {
        var result = await _mediator.Send(new ApproveGatePassCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

// ── Stock Demand ────────────────────────────────────────────────────

[ApiController]
[Route("api/inventory/stock-demands")]
[Authorize]
public class StockDemandController : ControllerBase
{
    private readonly IMediator _mediator;
    public StockDemandController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<StockDemandDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetStockDemandsQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockDemandDto>>> Create([FromBody] CreateStockDemandDto dto)
    {
        var result = await _mediator.Send(new CreateStockDemandCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

// ── Landed Cost ─────────────────────────────────────────────────────

[ApiController]
[Route("api/inventory/landed-costs")]
[Authorize]
public class LandedCostController : ControllerBase
{
    private readonly IMediator _mediator;
    public LandedCostController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LandedCostDto>>>> GetAll([FromQuery] int? purchaseInvoiceId)
        => Ok(await _mediator.Send(new GetLandedCostsQuery(purchaseInvoiceId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LandedCostDto>>> Create([FromBody] CreateLandedCostDto dto)
    {
        var result = await _mediator.Send(new CreateLandedCostCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
