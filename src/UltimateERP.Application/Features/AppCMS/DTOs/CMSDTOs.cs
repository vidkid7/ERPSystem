namespace UltimateERP.Application.Features.AppCMS.DTOs;

public class SliderDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? LinkURL { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSliderDto
{
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? LinkURL { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}

public class BannerDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? LinkURL { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBannerDto
{
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? LinkURL { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}

public class NoticeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateNoticeDto
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class UpdateSliderOrderDto
{
    public int SliderId { get; set; }
    public int NewOrder { get; set; }
}

public class ToggleActiveDto
{
    public int Id { get; set; }
}
