using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets;
using UltimateERP.Domain.Enums;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetTypeController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetTypeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetTypeDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? assetGroupId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAllAssetTypesQuery(search, assetGroupId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetTypeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateAssetTypeCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
