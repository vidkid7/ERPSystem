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
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentController(IMediator mediator) => _mediator = mediator;

    [HttpPost("initiate")]
    public async Task<ActionResult<ApiResponse<PaymentResultDto>>> Initiate([FromBody] PaymentRequestDto dto)
    {
        var result = await _mediator.Send(new InitiatePaymentCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("verify")]
    public async Task<ActionResult<ApiResponse<PaymentResultDto>>> Verify([FromBody] PaymentVerificationDto dto)
    {
        var result = await _mediator.Send(new VerifyPaymentCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
