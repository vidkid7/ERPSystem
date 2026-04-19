using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetGroupController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetGroupController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetGroupDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAllAssetGroupsQuery(search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateAssetGroupCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteAssetGroupCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
