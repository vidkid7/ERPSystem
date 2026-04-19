using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Task.Commands;
using UltimateERP.Application.Features.Task.DTOs;
using UltimateERP.Application.Features.Task.Queries;

namespace UltimateERP.API.Controllers.Task;

[ApiController]
[Route("api/task/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;
    public TaskController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TaskItemDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int? assignedToId, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetTasksQuery(status, assignedToId, search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> Create([FromBody] CreateTaskDto dto)
    {
        var result = await _mediator.Send(new CreateTaskCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> UpdateStatus(int id, [FromBody] UpdateTaskStatusDto dto)
    {
        dto.TaskId = id;
        var result = await _mediator.Send(new UpdateTaskStatusCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
