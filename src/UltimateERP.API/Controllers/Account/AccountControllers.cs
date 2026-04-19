using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.Commands;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Account.Queries;

namespace UltimateERP.API.Controllers.Account;

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class LedgerGroupController : ControllerBase
{
    private readonly IMediator _mediator;
    public LedgerGroupController(IMediator mediator) => _mediator = mediator;

    [HttpGet("tree")]
    public async Task<ActionResult<ApiResponse<List<LedgerGroupDto>>>> GetTree()
        => Ok(await _mediator.Send(new GetLedgerGroupTreeQuery()));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LedgerGroupDto>>> Create([FromBody] CreateLedgerGroupDto dto)
    {
        var result = await _mediator.Send(new CreateLedgerGroupCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class LedgerController : ControllerBase
{
    private readonly IMediator _mediator;
    public LedgerController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LedgerDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? groupId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetLedgersQuery(search, groupId, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LedgerDto>>> Create([FromBody] CreateLedgerDto dto)
    {
        var result = await _mediator.Send(new CreateLedgerCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class VoucherController : ControllerBase
{
    private readonly IMediator _mediator;
    public VoucherController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VoucherDto>>>> GetAll(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetVouchersQuery(fromDate, toDate, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<VoucherDto>>> Create([FromBody] CreateVoucherDto dto)
    {
        var result = await _mediator.Send(new CreateVoucherCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CustomerDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetCustomersQuery(search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Create([FromBody] CreateCustomerDto dto)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/account/[controller]")]
[Authorize]
public class VendorController : ControllerBase
{
    private readonly IMediator _mediator;
    public VendorController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VendorDto>>>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetVendorsQuery(search, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<VendorDto>>> Create([FromBody] CreateVendorDto dto)
    {
        var result = await _mediator.Send(new CreateVendorCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
