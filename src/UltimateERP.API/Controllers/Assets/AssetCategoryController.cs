using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetCategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetCategoryController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetCategoryDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAllAssetCategoriesQuery(search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateAssetCategoryCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
