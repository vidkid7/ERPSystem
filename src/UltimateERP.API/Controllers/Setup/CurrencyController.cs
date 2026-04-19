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
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;
    public CurrencyController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CurrencyDto>>>> GetAll([FromQuery] string? search)
        => Ok(await _mediator.Send(new GetCurrenciesQuery(search)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CurrencyDto>>> Create([FromBody] CreateCurrencyDto dto)
    {
        var result = await _mediator.Send(new CreateCurrencyCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("exchange-rates")]
    public async Task<ActionResult<ApiResponse<List<ExchangeRateDto>>>> GetExchangeRates(
        [FromQuery] int? fromCurrencyId, [FromQuery] int? toCurrencyId)
        => Ok(await _mediator.Send(new GetExchangeRatesQuery(fromCurrencyId, toCurrencyId)));

    [HttpPost("exchange-rates")]
    public async Task<ActionResult<ApiResponse<ExchangeRateDto>>> SetExchangeRate([FromBody] CreateExchangeRateDto dto)
    {
        var result = await _mediator.Send(new SetExchangeRateCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }

    [HttpGet("convert")]
    public async Task<ActionResult<ApiResponse<CurrencyConversionResultDto>>> Convert(
        [FromQuery] decimal amount, [FromQuery] int fromCurrencyId, [FromQuery] int toCurrencyId, [FromQuery] DateTime? asOfDate)
        => Ok(await _mediator.Send(new ConvertCurrencyQuery(amount, fromCurrencyId, toCurrencyId, asOfDate)));
}
