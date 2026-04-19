using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Dispatch.Commands;
using UltimateERP.Application.Features.Dispatch.DTOs;
using UltimateERP.Application.Features.Dispatch.Queries;

namespace UltimateERP.API.Controllers.Dispatch;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DispatchController : ControllerBase
{
    private readonly IMediator _mediator;
    public DispatchController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DispatchOrderDto>>>> GetDispatches(
        [FromQuery] string? status, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetDispatchesQuery(status, search, page, pageSize)));

    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<List<DispatchOrderDto>>>> GetPendingDispatches(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPendingDispatchesQuery(page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DispatchOrderDto>>> CreateDispatchOrder(
        [FromBody] CreateDispatchOrderDto dto)
    {
        var result = await _mediator.Send(new CreateDispatchOrderCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<DispatchOrderDto>>> ApproveDispatch(int id)
    {
        var result = await _mediator.Send(new ApproveDispatchCommand(new ApproveDispatchDto { DispatchOrderId = id }));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
