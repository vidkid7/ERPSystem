using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Sales.Commands;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Features.Sales.Queries;

namespace UltimateERP.API.Controllers.Sales;

[ApiController]
[Route("api/sales/[controller]")]
[Authorize]
public class SalesInvoiceController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesInvoiceController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesInvoiceDto>>>> GetAll(
        [FromQuery] int? customerId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSalesInvoicesQuery(customerId, fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesInvoiceDto>>> Create([FromBody] CreateSalesInvoiceDto dto)
    {
        var result = await _mediator.Send(new CreateSalesInvoiceCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
