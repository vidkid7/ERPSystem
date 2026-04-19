using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Lab.Commands;
using UltimateERP.Application.Features.Lab.DTOs;
using UltimateERP.Application.Features.Lab.Queries;

namespace UltimateERP.API.Controllers.Lab;

[ApiController]
[Route("api/lab/[controller]")]
[Authorize]
public class SampleController : ControllerBase
{
    private readonly IMediator _mediator;
    public SampleController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SampleCollectionDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSamplesQuery(search, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SampleCollectionDto>>> Create([FromBody] CreateSampleCollectionDto dto)
    {
        var result = await _mediator.Send(new CreateSampleCollectionCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/lab/[controller]")]
[Authorize]
public class LabReportController : ControllerBase
{
    private readonly IMediator _mediator;
    public LabReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LabReportDto>>>> GetAll(
        [FromQuery] int? sampleCollectionId, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetLabReportsQuery(sampleCollectionId, search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LabReportDto>>> Create([FromBody] CreateLabReportDto dto)
    {
        var result = await _mediator.Send(new CreateLabReportCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
