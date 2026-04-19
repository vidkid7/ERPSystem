using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HMS.Commands;
using UltimateERP.Application.Features.HMS.DTOs;
using UltimateERP.Application.Features.HMS.Queries;

namespace UltimateERP.API.Controllers.HMS;

[ApiController]
[Route("api/hms/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IMediator _mediator;
    public PatientController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PatientDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPatientsQuery(search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PatientDto>>> Register([FromBody] CreatePatientDto dto)
    {
        var result = await _mediator.Send(new RegisterPatientCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/hms/[controller]")]
[Authorize]
public class BedController : ControllerBase
{
    private readonly IMediator _mediator;
    public BedController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BedDto>>>> GetAll([FromQuery] string? status)
        => Ok(await _mediator.Send(new GetBedsQuery(status)));
}

[ApiController]
[Route("api/hms/[controller]")]
[Authorize]
public class OPDTicketController : ControllerBase
{
    private readonly IMediator _mediator;
    public OPDTicketController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<OPDTicketDto>>>> GetAll(
        [FromQuery] int? patientId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetOPDTicketsQuery(patientId, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OPDTicketDto>>> Create([FromBody] CreateOPDTicketDto dto)
    {
        var result = await _mediator.Send(new CreateOPDTicketCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/hms/[controller]")]
[Authorize]
public class IPDAdmissionController : ControllerBase
{
    private readonly IMediator _mediator;
    public IPDAdmissionController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<IPDAdmissionDto>>>> GetAll(
        [FromQuery] int? patientId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetIPDAdmissionsQuery(patientId, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IPDAdmissionDto>>> Create([FromBody] CreateIPDAdmissionDto dto)
    {
        var result = await _mediator.Send(new CreateIPDAdmissionCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id}/discharge")]
    public async Task<ActionResult<ApiResponse<IPDAdmissionDto>>> Discharge(int id, [FromQuery] int? dischargeTypeId)
    {
        var result = await _mediator.Send(new DischargePatientCommand(id, dischargeTypeId));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
