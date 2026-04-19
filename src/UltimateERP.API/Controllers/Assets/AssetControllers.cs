using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets.Commands;
using UltimateERP.Application.Features.Assets.DTOs;
using UltimateERP.Application.Features.Assets.Queries;

namespace UltimateERP.API.Controllers.Assets;

[ApiController]
[Route("api/assets/[controller]")]
[Authorize]
public class AssetMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetMasterController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAssetsQuery(search, status, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AssetDto>>> Register([FromBody] RegisterAssetDto dto)
    {
        var result = await _mediator.Send(new RegisterAssetCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPost("{id}/depreciation")]
    public async Task<ActionResult<ApiResponse<DepreciationResultDto>>> CalculateDepreciation(
        int id, [FromQuery] decimal usefulLifeYears = 10)
    {
        var result = await _mediator.Send(new CalculateDepreciationCommand(id, usefulLifeYears));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
