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
public class SSFController : ControllerBase
{
    private readonly IMediator _mediator;
    public SSFController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<SSFRegistrationResultDto>>> Register([FromBody] SSFRegistrationRequestDto dto)
    {
        var result = await _mediator.Send(new RegisterSSFEmployeeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("contribute")]
    public async Task<ActionResult<ApiResponse<SSFContributionResultDto>>> Contribute([FromBody] SSFContributionRequestDto dto)
    {
        var result = await _mediator.Send(new SubmitSSFContributionCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
