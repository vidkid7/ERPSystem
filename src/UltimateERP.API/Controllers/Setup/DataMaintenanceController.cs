using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.Commands;
using UltimateERP.Application.Features.Setup.DTOs;

namespace UltimateERP.API.Controllers.Setup;

[ApiController]
[Route("api/setup/[controller]")]
[Authorize]
public class DataMaintenanceController : ControllerBase
{
    private readonly IMediator _mediator;
    public DataMaintenanceController(IMediator mediator) => _mediator = mediator;

    [HttpPost("merge-ledgers")]
    public async Task<ActionResult<ApiResponse<MergeResultDto>>> MergeLedgers([FromBody] MergeLedgersDto dto)
    {
        var result = await _mediator.Send(new MergeLedgersCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("merge-products")]
    public async Task<ActionResult<ApiResponse<MergeResultDto>>> MergeProducts([FromBody] MergeProductsDto dto)
    {
        var result = await _mediator.Send(new MergeProductsCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("renumber-vouchers")]
    public async Task<ActionResult<ApiResponse<MergeResultDto>>> RenumberVouchers([FromBody] RenumberVouchersDto dto)
    {
        var result = await _mediator.Send(new RenumberVouchersCommand(dto));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
