using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.Commands;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Features.Purchase.Queries;

namespace UltimateERP.API.Controllers.Purchase;

[ApiController]
[Route("api/purchase/[controller]")]
[Authorize]
public class PurchaseInvoiceController : ControllerBase
{
    private readonly IMediator _mediator;
    public PurchaseInvoiceController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PurchaseInvoiceDto>>>> GetAll(
        [FromQuery] int? vendorId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPurchaseInvoicesQuery(vendorId, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> Create([FromBody] CreatePurchaseInvoiceDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseInvoiceCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
