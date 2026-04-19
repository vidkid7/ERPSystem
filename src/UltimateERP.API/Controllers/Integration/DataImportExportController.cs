using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Integration.Commands;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Application.Features.Integration.Queries;

namespace UltimateERP.API.Controllers.Integration;

[ApiController]
[Route("api/integration/[controller]")]
[Authorize]
public class DataImportExportController : ControllerBase
{
    private readonly IMediator _mediator;
    public DataImportExportController(IMediator mediator) => _mediator = mediator;

    [HttpPost("import/ledgers")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportLedgers(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<ImportResultDto>.Failure("File is required"));

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new ImportLedgersCommand(stream));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("import/products")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> ImportProducts(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<ImportResultDto>.Failure("File is required"));

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new ImportProductsCommand(stream));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("export/ledgers")]
    public async Task<IActionResult> ExportLedgers()
    {
        var result = await _mediator.Send(new ExportLedgersQuery());
        if (!result.IsSuccess) return BadRequest(result);
        return File(result.Data!, "text/csv", "ledgers.csv");
    }

    [HttpGet("export/products")]
    public async Task<IActionResult> ExportProducts()
    {
        var result = await _mediator.Send(new ExportProductsQuery());
        if (!result.IsSuccess) return BadRequest(result);
        return File(result.Data!, "text/csv", "products.csv");
    }
}
