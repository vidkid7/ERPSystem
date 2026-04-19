using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.Commands;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Queries;

namespace UltimateERP.API.Controllers.Reporting;

[ApiController]
[Route("api/reports/builder")]
[Authorize]
public class ReportBuilderController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReportBuilderController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ReportDefinitionDto>>>> GetReportDefinitions(
        [FromQuery] int? userId)
        => Ok(await _mediator.Send(new GetReportDefinitionsQuery(userId)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReportDefinitionDto>>> CreateReportDefinition(
        [FromBody] CreateReportDefinitionDto dto)
    {
        var result = await _mediator.Send(new CreateReportDefinitionCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<ReportDefinitionDto>>> UpdateReportDefinition(
        [FromBody] UpdateReportDefinitionDto dto)
    {
        var result = await _mediator.Send(new UpdateReportDefinitionCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteReportDefinition(int id)
    {
        var result = await _mediator.Send(new DeleteReportDefinitionCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("execute/{reportDefinitionId}")]
    public async Task<ActionResult<ApiResponse<CustomReportResultDto>>> ExecuteCustomReport(int reportDefinitionId)
        => Ok(await _mediator.Send(new ExecuteCustomReportQuery(reportDefinitionId)));
}
