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

    [HttpPut("{id}/assign")]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> AssignTask(int id, [FromBody] AssignTaskDto dto)
    {
        dto.TaskId = id;
        var result = await _mediator.Send(new AssignTaskCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id}/comments")]
    public async Task<ActionResult<ApiResponse<TaskCommentDto>>> AddComment(int id, [FromBody] AddTaskCommentDto dto)
    {
        dto.TaskItemId = id;
        var result = await _mediator.Send(new AddTaskCommentCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("by-assignee/{assignedToId}")]
    public async Task<ActionResult<ApiResponse<List<TaskItemDto>>>> GetByAssignee(
        int assignedToId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetTasksByAssigneeQuery(assignedToId, page, pageSize)));

    [HttpGet("by-date-range")]
    public async Task<ActionResult<ApiResponse<List<TaskItemDto>>>> GetByDateRange(
        [FromQuery] DateTime from, [FromQuery] DateTime to,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetTasksByDateRangeQuery(from, to, page, pageSize)));

    [HttpGet("overdue")]
    public async Task<ActionResult<ApiResponse<List<TaskItemDto>>>> GetOverdue(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetOverdueTasksQuery(page, pageSize)));
}
