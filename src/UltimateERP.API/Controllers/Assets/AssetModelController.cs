using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetModelController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetModelController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetModelDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? assetTypeId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAllAssetModelsQuery(search, assetTypeId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetModelCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateAssetModelCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
