using AutoMapper;
using FluentValidation;
using MediatR;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.AppCMS.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.CMS;

namespace UltimateERP.Application.Features.AppCMS.Commands;

// Create Slider
public record CreateSliderCommand(CreateSliderDto Slider) : IRequest<ApiResponse<SliderDto>>;

public class CreateSliderValidator : AbstractValidator<CreateSliderCommand>
{
    public CreateSliderValidator()
    {
        RuleFor(x => x.Slider.Title).NotEmpty().MaximumLength(200);
    }
}

public class CreateSliderHandler : IRequestHandler<CreateSliderCommand, ApiResponse<SliderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSliderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SliderDto>> Handle(CreateSliderCommand request, CancellationToken ct)
    {
        var dto = request.Slider;
        var slider = new Slider
        {
            Title = dto.Title,
            ImagePath = dto.ImagePath,
            LinkURL = dto.LinkURL,
            DisplayOrder = dto.DisplayOrder,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo
        };

        _db.Sliders.Add(slider);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SliderDto>.Success(_mapper.Map<SliderDto>(slider), "Slider created");
    }
}

// Create Banner
public record CreateBannerCommand(CreateBannerDto Banner) : IRequest<ApiResponse<BannerDto>>;

public class CreateBannerValidator : AbstractValidator<CreateBannerCommand>
{
    public CreateBannerValidator()
    {
        RuleFor(x => x.Banner.Title).NotEmpty().MaximumLength(200);
    }
}

public class CreateBannerHandler : IRequestHandler<CreateBannerCommand, ApiResponse<BannerDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateBannerHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BannerDto>> Handle(CreateBannerCommand request, CancellationToken ct)
    {
        var dto = request.Banner;
        var banner = new Banner
        {
            Title = dto.Title,
            ImagePath = dto.ImagePath,
            LinkURL = dto.LinkURL,
            DisplayOrder = dto.DisplayOrder,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo
        };

        _db.Banners.Add(banner);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<BannerDto>.Success(_mapper.Map<BannerDto>(banner), "Banner created");
    }
}

// Create Notice
public record CreateNoticeCommand(CreateNoticeDto Notice) : IRequest<ApiResponse<NoticeDto>>;

public class CreateNoticeValidator : AbstractValidator<CreateNoticeCommand>
{
    public CreateNoticeValidator()
    {
        RuleFor(x => x.Notice.Title).NotEmpty().MaximumLength(200);
    }
}

public class CreateNoticeHandler : IRequestHandler<CreateNoticeCommand, ApiResponse<NoticeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateNoticeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<NoticeDto>> Handle(CreateNoticeCommand request, CancellationToken ct)
    {
        var dto = request.Notice;
        var notice = new Notice
        {
            Title = dto.Title,
            Content = dto.Content,
            PublishDate = dto.PublishDate,
            ExpiryDate = dto.ExpiryDate
        };

        _db.Notices.Add(notice);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<NoticeDto>.Success(_mapper.Map<NoticeDto>(notice), "Notice created");
    }
}

// Update Slider Order
public record UpdateSliderOrderCommand(UpdateSliderOrderDto Dto) : IRequest<ApiResponse<SliderDto>>;

public class UpdateSliderOrderHandler : IRequestHandler<UpdateSliderOrderCommand, ApiResponse<SliderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateSliderOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SliderDto>> Handle(UpdateSliderOrderCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var slider = await _db.Sliders.FindAsync(new object[] { dto.SliderId }, ct);
        if (slider is null) return ApiResponse<SliderDto>.Failure("Slider not found");

        slider.DisplayOrder = dto.NewOrder;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SliderDto>.Success(_mapper.Map<SliderDto>(slider), "Slider order updated");
    }
}

// Toggle Slider Active
public record ToggleSliderActiveCommand(ToggleActiveDto Dto) : IRequest<ApiResponse<SliderDto>>;

public class ToggleSliderActiveHandler : IRequestHandler<ToggleSliderActiveCommand, ApiResponse<SliderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ToggleSliderActiveHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SliderDto>> Handle(ToggleSliderActiveCommand request, CancellationToken ct)
    {
        var slider = await _db.Sliders.FindAsync(new object[] { request.Dto.Id }, ct);
        if (slider is null) return ApiResponse<SliderDto>.Failure("Slider not found");

        slider.IsActive = !slider.IsActive;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SliderDto>.Success(_mapper.Map<SliderDto>(slider), "Slider active status toggled");
    }
}

// Toggle Banner Active
public record ToggleBannerActiveCommand(ToggleActiveDto Dto) : IRequest<ApiResponse<BannerDto>>;

public class ToggleBannerActiveHandler : IRequestHandler<ToggleBannerActiveCommand, ApiResponse<BannerDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ToggleBannerActiveHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BannerDto>> Handle(ToggleBannerActiveCommand request, CancellationToken ct)
    {
        var banner = await _db.Banners.FindAsync(new object[] { request.Dto.Id }, ct);
        if (banner is null) return ApiResponse<BannerDto>.Failure("Banner not found");

        banner.IsActive = !banner.IsActive;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<BannerDto>.Success(_mapper.Map<BannerDto>(banner), "Banner active status toggled");
    }
}
