using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Lab.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Lab;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Lab.Commands;

// Create Sample Collection
public record CreateSampleCollectionCommand(CreateSampleCollectionDto Sample) : IRequest<ApiResponse<SampleCollectionDto>>;

public class CreateSampleCollectionValidator : AbstractValidator<CreateSampleCollectionCommand>
{
    public CreateSampleCollectionValidator()
    {
        RuleFor(x => x.Sample.SampleNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Sample.PatientName).NotEmpty().MaximumLength(200);
    }
}

public class CreateSampleCollectionHandler : IRequestHandler<CreateSampleCollectionCommand, ApiResponse<SampleCollectionDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSampleCollectionHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SampleCollectionDto>> Handle(CreateSampleCollectionCommand request, CancellationToken ct)
    {
        var dto = request.Sample;
        var exists = await _db.SampleCollections.AnyAsync(s => s.SampleNumber == dto.SampleNumber, ct);
        if (exists) return ApiResponse<SampleCollectionDto>.Failure($"Sample number {dto.SampleNumber} already exists");

        var sample = new SampleCollection
        {
            SampleNumber = dto.SampleNumber,
            CollectionDate = dto.CollectionDate,
            CollectionDateBS = dto.CollectionDateBS,
            PatientName = dto.PatientName,
            PatientAge = dto.PatientAge,
            PatientGender = dto.PatientGender,
            PatientContact = dto.PatientContact,
            DoctorName = dto.DoctorName,
            TestParameters = dto.TestParameters,
            Status = SampleCollectionStatus.Pending
        };

        _db.SampleCollections.Add(sample);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SampleCollectionDto>.Success(_mapper.Map<SampleCollectionDto>(sample), "Sample collection created");
    }
}

// Create Lab Report
public record CreateLabReportCommand(CreateLabReportDto Report) : IRequest<ApiResponse<LabReportDto>>;

public class CreateLabReportValidator : AbstractValidator<CreateLabReportCommand>
{
    public CreateLabReportValidator()
    {
        RuleFor(x => x.Report.SampleCollectionId).GreaterThan(0);
    }
}

public class CreateLabReportHandler : IRequestHandler<CreateLabReportCommand, ApiResponse<LabReportDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateLabReportHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LabReportDto>> Handle(CreateLabReportCommand request, CancellationToken ct)
    {
        var dto = request.Report;
        var sample = await _db.SampleCollections.FindAsync(new object[] { dto.SampleCollectionId }, ct);
        if (sample is null) return ApiResponse<LabReportDto>.Failure("Sample collection not found");

        var report = new LabReport
        {
            SampleCollectionId = dto.SampleCollectionId,
            ReportDate = dto.ReportDate,
            ReportDateBS = dto.ReportDateBS,
            TemplateId = dto.TemplateId,
            ReportData = dto.ReportData,
            GeneratedBy = dto.GeneratedBy
        };

        sample.Status = SampleCollectionStatus.Completed;

        _db.LabReports.Add(report);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<LabReportDto>.Success(_mapper.Map<LabReportDto>(report), "Lab report created");
    }
}
