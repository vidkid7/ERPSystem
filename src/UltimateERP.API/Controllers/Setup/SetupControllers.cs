using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.Commands;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Features.Setup.Queries;

namespace UltimateERP.API.Controllers.Setup;

[ApiController]
[Route("api/setup/[controller]")]
[Authorize]
public class BranchController : ControllerBase
{
    private readonly IMediator _mediator;
    public BranchController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BranchDto>>>> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetBranchesQuery(search, page, pageSize)));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BranchDto>>> GetById(int id)
    {
        var result = await _mediator.Send(new GetBranchByIdQuery(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BranchDto>>> Create([FromBody] CreateBranchDto dto)
    {
        var result = await _mediator.Send(new CreateBranchCommand(dto));
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<BranchDto>>> Update(int id, [FromBody] CreateBranchDto dto)
    {
        var result = await _mediator.Send(new UpdateBranchCommand(id, dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteBranchCommand(id));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}

[ApiController]
[Route("api/setup/[controller]")]
[Authorize]
public class CostClassController : ControllerBase
{
    private readonly IMediator _mediator;
    public CostClassController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CostClassDto>>>> GetAll([FromQuery] string? search)
        => Ok(await _mediator.Send(new GetCostClassesQuery(search)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CostClassDto>>> Create([FromBody] CreateCostClassDto dto)
    {
        var result = await _mediator.Send(new CreateCostClassCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/setup/[controller]")]
[Authorize]
public class DocumentTypeController : ControllerBase
{
    private readonly IMediator _mediator;
    public DocumentTypeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DocumentTypeDto>>>> GetAll([FromQuery] string? module)
        => Ok(await _mediator.Send(new GetDocumentTypesQuery(module)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DocumentTypeDto>>> Create([FromBody] CreateDocumentTypeDto dto)
    {
        var result = await _mediator.Send(new CreateDocumentTypeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/setup/[controller]")]
[Authorize]
public class EntityNumberingController : ControllerBase
{
    private readonly IMediator _mediator;
    public EntityNumberingController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EntityNumberingDto>>>> GetAll()
        => Ok(await _mediator.Send(new GetEntityNumberingsQuery()));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EntityNumberingDto>>> Create([FromBody] CreateEntityNumberingDto dto)
    {
        var result = await _mediator.Send(new CreateEntityNumberingCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
