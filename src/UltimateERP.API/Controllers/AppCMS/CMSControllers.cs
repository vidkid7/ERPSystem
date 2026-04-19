using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.AppCMS.Commands;
using UltimateERP.Application.Features.AppCMS.DTOs;
using UltimateERP.Application.Features.AppCMS.Queries;

namespace UltimateERP.API.Controllers.AppCMS;

[ApiController]
[Route("api/cms/[controller]")]
[Authorize]
public class SliderController : ControllerBase
{
    private readonly IMediator _mediator;
    public SliderController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SliderDto>>>> GetAll(
        [FromQuery] bool? activeOnly, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetSlidersQuery(activeOnly, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SliderDto>>> Create([FromBody] CreateSliderDto dto)
    {
        var result = await _mediator.Send(new CreateSliderCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/cms/[controller]")]
[Authorize]
public class BannerController : ControllerBase
{
    private readonly IMediator _mediator;
    public BannerController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BannerDto>>>> GetAll(
        [FromQuery] bool? activeOnly, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetBannersQuery(activeOnly, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Create([FromBody] CreateBannerDto dto)
    {
        var result = await _mediator.Send(new CreateBannerCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}

[ApiController]
[Route("api/cms/[controller]")]
[Authorize]
public class NoticeController : ControllerBase
{
    private readonly IMediator _mediator;
    public NoticeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<NoticeDto>>>> GetAll(
        [FromQuery] bool? activeOnly, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetNoticesQuery(activeOnly, page, pageSize)));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<NoticeDto>>> Create([FromBody] CreateNoticeDto dto)
    {
        var result = await _mediator.Send(new CreateNoticeCommand(dto));
        return result.IsSuccess ? Created("", result) : BadRequest(result);
    }
}
