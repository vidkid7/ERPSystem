using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Industry.Commands;
using UltimateERP.Application.Features.Industry.DTOs;
using UltimateERP.Application.Features.Industry.Queries;

namespace UltimateERP.API.Controllers.Industry;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IndustryController : ControllerBase
{
    private readonly IMediator _mediator;
    public IndustryController(IMediator mediator) => _mediator = mediator;

    [HttpGet("dairy-purchases")]
    public async Task<ActionResult<ApiResponse<List<DairyPurchaseInvoiceDto>>>> GetDairyPurchases(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetDairyPurchasesQuery(search, page, pageSize)));

    [HttpPost("dairy-purchase")]
    public async Task<ActionResult<ApiResponse<DairyPurchaseInvoiceDto>>> CreateDairyPurchase(
        [FromBody] CreateDairyPurchaseDto dto)
    {
        var result = await _mediator.Send(new CreateDairyPurchaseCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("tea-purchases")]
    public async Task<ActionResult<ApiResponse<List<TeaPurchaseInvoiceDto>>>> GetTeaPurchases(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetTeaPurchasesQuery(search, page, pageSize)));

    [HttpPost("tea-purchase")]
    public async Task<ActionResult<ApiResponse<TeaPurchaseInvoiceDto>>> CreateTeaPurchase(
        [FromBody] CreateTeaPurchaseDto dto)
    {
        var result = await _mediator.Send(new CreateTeaPurchaseCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("petrol-transactions")]
    public async Task<ActionResult<ApiResponse<List<PetrolPumpTransactionDto>>>> GetPetrolPumpTransactions(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetPetrolPumpTransactionsQuery(search, page, pageSize)));

    [HttpPost("petrol-transaction")]
    public async Task<ActionResult<ApiResponse<PetrolPumpTransactionDto>>> CreatePetrolPumpTransaction(
        [FromBody] CreatePetrolPumpTransactionDto dto)
    {
        var result = await _mediator.Send(new CreatePetrolPumpTransactionCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
