using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Support.Commands;
using UltimateERP.Application.Features.Support.DTOs;
using UltimateERP.Application.Features.Support.Queries;

namespace UltimateERP.API.Controllers.Support;

[ApiController]
[Route("api/support/[controller]")]
[Authorize]
public class SupportTicketController : ControllerBase
{
    private readonly IMediator _mediator;
    public SupportTicketController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SupportTicketDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] string? search, [FromQuery] int? assignedToId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSupportTicketsQuery(status, search, assignedToId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SupportTicketDto>>> Create([FromBody] CreateSupportTicketDto dto)
    {
        var result = await _mediator.Send(new CreateSupportTicketCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id}/assign")]
    public async Task<ActionResult<ApiResponse<SupportTicketDto>>> Assign(int id, [FromBody] AssignSupportTicketDto dto)
    {
        dto.TicketId = id;
        var result = await _mediator.Send(new AssignSupportTicketCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}/resolve")]
    public async Task<ActionResult<ApiResponse<SupportTicketDto>>> Resolve(int id, [FromBody] ResolveSupportTicketDto dto)
    {
        dto.TicketId = id;
        var result = await _mediator.Send(new ResolveSupportTicketCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}/escalate")]
    public async Task<ActionResult<ApiResponse<SupportTicketDto>>> Escalate(int id, [FromBody] EscalateSupportTicketDto dto)
    {
        dto.TicketId = id;
        var result = await _mediator.Send(new EscalateSupportTicketCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
