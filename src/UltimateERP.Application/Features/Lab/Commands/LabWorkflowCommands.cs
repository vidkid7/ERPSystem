using MediatR;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Services;

namespace UltimateERP.Application.Features.Lab.Commands;

// Process Sample Command (Collected → InProcess)
public record ProcessSampleCommand(int SampleId) : IRequest<ApiResponse<string>>;

public class ProcessSampleHandler : IRequestHandler<ProcessSampleCommand, ApiResponse<string>>
{
    private readonly LabWorkflowService _workflow;
    public ProcessSampleHandler(LabWorkflowService workflow) => _workflow = workflow;

    public async Task<ApiResponse<string>> Handle(ProcessSampleCommand request, CancellationToken ct)
    {
        var (success, message) = await _workflow.ProcessSample(request.SampleId, ct);
        return success
            ? ApiResponse<string>.Success(message, message)
            : ApiResponse<string>.Failure(message);
    }
}

// Complete Lab Report Command
public record CompleteLabReportCommand(int ReportId, string Results) : IRequest<ApiResponse<string>>;

public class CompleteLabReportHandler : IRequestHandler<CompleteLabReportCommand, ApiResponse<string>>
{
    private readonly LabWorkflowService _workflow;
    public CompleteLabReportHandler(LabWorkflowService workflow) => _workflow = workflow;

    public async Task<ApiResponse<string>> Handle(CompleteLabReportCommand request, CancellationToken ct)
    {
        var (success, message) = await _workflow.CompleteReport(request.ReportId, request.Results, ct);
        return success
            ? ApiResponse<string>.Success(message, message)
            : ApiResponse<string>.Failure(message);
    }
}

// Validate Lab Report Command
public record ValidateLabReportCommand(int ReportId, int ValidatedBy) : IRequest<ApiResponse<string>>;

public class ValidateLabReportHandler : IRequestHandler<ValidateLabReportCommand, ApiResponse<string>>
{
    private readonly LabWorkflowService _workflow;
    public ValidateLabReportHandler(LabWorkflowService workflow) => _workflow = workflow;

    public async Task<ApiResponse<string>> Handle(ValidateLabReportCommand request, CancellationToken ct)
    {
        var (success, message) = await _workflow.ValidateReport(request.ReportId, request.ValidatedBy, ct);
        return success
            ? ApiResponse<string>.Success(message, message)
            : ApiResponse<string>.Failure(message);
    }
}
