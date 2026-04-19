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
public class AssetController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssetController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetItemDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? status,
        [FromQuery] int? categoryId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        AssetStatus? statusEnum = Enum.TryParse<AssetStatus>(status, out var parsed) ? parsed : null;
        return Ok(await _mediator.Send(new GetAllAssetsQuery(search, statusEnum, categoryId, page, pageSize)));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAssetCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateAssetCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}/transactions")]
    public async Task<ActionResult<ApiResponse<List<AssetTransactionDto>>>> GetTransactions(
        int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetAssetTransactionsQuery(id, page, pageSize)));
}
