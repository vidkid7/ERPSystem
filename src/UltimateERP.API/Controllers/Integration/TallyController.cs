using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Integration.Commands;
using UltimateERP.Application.Features.Integration.DTOs;

namespace UltimateERP.API.Controllers.Integration;

[ApiController]
[Route("api/integration/[controller]")]
[Authorize]
public class TallyController : ControllerBase
{
    private readonly IMediator _mediator;
    public TallyController(IMediator mediator) => _mediator = mediator;

    [HttpPost("import")]
    public async Task<ActionResult<ApiResponse<ImportResultDto>>> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse<ImportResultDto>.Failure("XML file is required"));

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new ImportFromTallyCommand(stream));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] TallyExportRequestDto dto)
    {
        var result = await _mediator.Send(new ExportToTallyCommand(dto));
        if (!result.IsSuccess) return BadRequest(result);
        return File(result.Data!, "application/xml", $"tally-export-{dto.ExportType}.xml");
    }
}
