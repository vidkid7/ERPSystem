using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Integration.Commands;
using UltimateERP.Application.Features.Integration.DTOs;

namespace UltimateERP.API.Controllers.Integration;

[ApiController]
[Route("api/integration/[controller]")]
[Authorize]
public class SmsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SmsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("send")]
    public async Task<ActionResult<ApiResponse<bool>>> SendSms([FromBody] SendSmsDto dto)
    {
        var result = await _mediator.Send(new SendSmsCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("send-bulk")]
    public async Task<ActionResult<ApiResponse<bool>>> SendBulkSms([FromBody] SendBulkSmsDto dto)
    {
        var result = await _mediator.Send(new SendBulkSmsCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
