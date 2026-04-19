using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Manufacturing.Commands;
using UltimateERP.Application.Features.Manufacturing.DTOs;
using UltimateERP.Application.Features.Manufacturing.Queries;

namespace UltimateERP.API.Controllers.Manufacturing;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ManufacturingController : ControllerBase
{
    private readonly IMediator _mediator;
    public ManufacturingController(IMediator mediator) => _mediator = mediator;

    [HttpGet("boms")]
    public async Task<ActionResult<ApiResponse<List<BOMDto>>>> GetBOMs(
        [FromQuery] int? productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetBOMsQuery(productId, page, pageSize)));

    [HttpPost("bom")]
    public async Task<ActionResult<ApiResponse<BOMDto>>> CreateBOM([FromBody] CreateBOMDto dto)
    {
        var result = await _mediator.Send(new CreateBOMCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("production-orders")]
    public async Task<ActionResult<ApiResponse<List<ProductionOrderDto>>>> GetProductionOrders(
        [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetProductionOrdersQuery(status, page, pageSize)));

    [HttpPost("production-order")]
    public async Task<ActionResult<ApiResponse<ProductionOrderDto>>> CreateProductionOrder([FromBody] CreateProductionOrderDto dto)
    {
        var result = await _mediator.Send(new CreateProductionOrderCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("production-order/{id}/complete")]
    public async Task<ActionResult<ApiResponse<ProductionOrderDto>>> CompleteProductionOrder(
        int id, [FromBody] CompleteProductionOrderDto dto)
    {
        dto.ProductionOrderId = id;
        var result = await _mediator.Send(new CompleteProductionOrderCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
