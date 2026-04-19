using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Nepal.Commands;
using UltimateERP.Application.Features.Nepal.DTOs;
using UltimateERP.Application.Features.Nepal.Queries;

namespace UltimateERP.API.Controllers.Nepal;

[ApiController]
[Route("api/nepal/[controller]")]
[Authorize]
public class IRDController : ControllerBase
{
    private readonly IMediator _mediator;
    public IRDController(IMediator mediator) => _mediator = mediator;

    [HttpPost("submit-sales")]
    public async Task<ActionResult<ApiResponse<IRDSubmissionResultDto>>> SubmitSales([FromBody] IRDSalesDataDto dto)
    {
        var result = await _mediator.Send(new SubmitSalesToIRDCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("submit-purchase")]
    public async Task<ActionResult<ApiResponse<IRDSubmissionResultDto>>> SubmitPurchase([FromBody] IRDPurchaseDataDto dto)
    {
        var result = await _mediator.Send(new SubmitPurchaseToIRDCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("annex10")]
    public async Task<ActionResult<ApiResponse<List<Annex10ItemDto>>>> GetAnnex10(
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        => Ok(await _mediator.Send(new GetAnnex10VATReportQuery(fromDate, toDate)));

    [HttpGet("excise-register")]
    public async Task<ActionResult<ApiResponse<List<ExciseRegisterItemDto>>>> GetExciseRegister(
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        => Ok(await _mediator.Send(new GetExciseRegisterQuery(fromDate, toDate)));

    [HttpGet("one-lakh-above")]
    public async Task<ActionResult<ApiResponse<List<OneLakhAboveDto>>>> GetOneLakhAbove(
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        => Ok(await _mediator.Send(new GetOneLakhAboveSalesReportQuery(fromDate, toDate)));
}
