using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.Commands;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Features.Inventory.Queries;

namespace UltimateERP.API.Controllers.Inventory;

[ApiController]
[Route("api/inventory/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? groupId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetProductsQuery(search, groupId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(new CreateProductCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(new UpdateProductCommand(id, dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/inventory/[controller]")]
[Authorize]
public class ProductGroupController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductGroupController(IMediator mediator) => _mediator = mediator;

    [HttpGet("tree")]
    public async Task<ActionResult<ApiResponse<List<ProductGroupDto>>>> GetTree()
        => Ok(await _mediator.Send(new GetProductGroupTreeQuery()));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductGroupDto>>> Create([FromBody] CreateProductGroupDto dto)
    {
        var result = await _mediator.Send(new CreateProductGroupCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/inventory/[controller]")]
[Authorize]
public class GodownController : ControllerBase
{
    private readonly IMediator _mediator;
    public GodownController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GodownDto>>>> GetAll()
        => Ok(await _mediator.Send(new GetGodownsQuery()));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GodownDto>>> Create([FromBody] CreateGodownDto dto)
    {
        var result = await _mediator.Send(new CreateGodownCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/inventory/[controller]")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;
    public StockController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<StockDto>>>> GetStock(
        [FromQuery] int? productId, [FromQuery] int? godownId)
        => Ok(await _mediator.Send(new GetStockQuery(productId, godownId)));
}
