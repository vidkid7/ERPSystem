using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Service.Commands;
using UltimateERP.Application.Features.Service.DTOs;
using UltimateERP.Application.Features.Service.Queries;

namespace UltimateERP.API.Controllers.Service;

[ApiController]
[Route("api/service/[controller]")]
[Authorize]
public class ComplaintTicketController : ControllerBase
{
    private readonly IMediator _mediator;
    public ComplaintTicketController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ComplaintTicketDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetComplaintTicketsQuery(customerId, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ComplaintTicketDto>>> Create([FromBody] CreateComplaintTicketDto dto)
    {
        var result = await _mediator.Send(new CreateComplaintTicketCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/service/[controller]")]
[Authorize]
public class JobCardController : ControllerBase
{
    private readonly IMediator _mediator;
    public JobCardController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<JobCardDto>>>> GetAll(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetJobCardsQuery(status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JobCardDto>>> Create([FromBody] CreateJobCardDto dto)
    {
        var result = await _mediator.Send(new CreateJobCardCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id}/complete")]
    public async Task<ActionResult<ApiResponse<JobCardDto>>> Complete(int id, [FromQuery] decimal actualCost)
    {
        var result = await _mediator.Send(new CompleteJobCardCommand(id, actualCost));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/service/[controller]")]
[Authorize]
public class ServiceAppointmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServiceAppointmentController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ServiceAppointmentDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetServiceAppointmentsQuery(customerId, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ServiceAppointmentDto>>> Create([FromBody] CreateServiceAppointmentDto dto)
    {
        var result = await _mediator.Send(new CreateServiceAppointmentCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
