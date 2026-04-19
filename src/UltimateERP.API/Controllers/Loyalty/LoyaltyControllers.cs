using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Loyalty.Commands;
using UltimateERP.Application.Features.Loyalty.DTOs;
using UltimateERP.Application.Features.Loyalty.Queries;

namespace UltimateERP.API.Controllers.Loyalty;

[ApiController]
[Route("api/loyalty/[controller]")]
[Authorize]
public class LoyaltyController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoyaltyController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{customerId}")]
    public async Task<ActionResult<ApiResponse<List<MembershipPointDto>>>> GetMemberPoints(
        int customerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetMemberPointsQuery(customerId, page, pageSize)));

    [HttpPost("accrue")]
    public async Task<ActionResult<ApiResponse<MembershipPointDto>>> AccruePoints([FromBody] AccruePointsDto dto)
    {
        var result = await _mediator.Send(new AccruePointsCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("redeem")]
    public async Task<ActionResult<ApiResponse<MembershipPointDto>>> RedeemPoints([FromBody] RedeemPointsDto dto)
    {
        var result = await _mediator.Send(new RedeemPointsCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{customerId}/balance")]
    public async Task<ActionResult<ApiResponse<PointsBalanceDto>>> GetPointsBalance(int customerId)
        => Ok(await _mediator.Send(new GetPointsBalanceQuery(customerId)));

    [HttpGet("{customerId}/history")]
    public async Task<ActionResult<ApiResponse<List<MembershipPointDto>>>> GetPointsHistory(
        int customerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPointsHistoryQuery(customerId, from, to, page, pageSize)));
}
