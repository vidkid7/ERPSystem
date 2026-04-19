using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.KYC.Commands;
using UltimateERP.Application.Features.KYC.DTOs;
using UltimateERP.Application.Features.KYC.Queries;

namespace UltimateERP.API.Controllers.KYC;

[ApiController]
[Route("api/kyc/[controller]")]
[Authorize]
public class KYCController : ControllerBase
{
    private readonly IMediator _mediator;
    public KYCController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<KYCRecordDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetKYCRecordsQuery(search, page, pageSize)));

    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<List<KYCRecordDto>>>> GetPending(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPendingKYCQuery(page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<KYCRecordDto>>> Create([FromBody] CreateKYCDto dto)
    {
        var result = await _mediator.Send(new CreateKYCCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<KYCRecordDto>>> Update([FromBody] UpdateKYCDto dto)
    {
        var result = await _mediator.Send(new UpdateKYCCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("verify")]
    public async Task<ActionResult<ApiResponse<KYCRecordDto>>> Verify([FromBody] VerifyKYCDto dto)
    {
        var result = await _mediator.Send(new VerifyKYCCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
