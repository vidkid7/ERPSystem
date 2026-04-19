using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetTransactionController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetTransactionController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{assetId}")]
    public async Task<ActionResult<ApiResponse<List<AssetTransactionDto>>>> GetByAsset(
        int assetId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAssetTransactionsQuery(assetId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetTransactionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
