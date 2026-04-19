using MediatR;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Integration.Commands;

// ── SMS Commands ──────────────────────────────────────────────────────

public record SendSmsCommand(SendSmsDto Sms) : IRequest<ApiResponse<bool>>;

public class SendSmsHandler : IRequestHandler<SendSmsCommand, ApiResponse<bool>>
{
    private readonly ISmsService _smsService;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(ISmsService smsService, ILogger<SendSmsHandler> logger)
    {
        _smsService = smsService;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(SendSmsCommand request, CancellationToken ct)
    {
        var result = await _smsService.SendSmsAsync(request.Sms.PhoneNumber, request.Sms.Message);
        _logger.LogInformation("SMS sent to {Phone}: {Result}", request.Sms.PhoneNumber, result);
        return result
            ? ApiResponse<bool>.Success(true, "SMS sent successfully")
            : ApiResponse<bool>.Failure("Failed to send SMS");
    }
}

public record SendBulkSmsCommand(SendBulkSmsDto BulkSms) : IRequest<ApiResponse<bool>>;

public class SendBulkSmsHandler : IRequestHandler<SendBulkSmsCommand, ApiResponse<bool>>
{
    private readonly ISmsService _smsService;
    private readonly ILogger<SendBulkSmsHandler> _logger;

    public SendBulkSmsHandler(ISmsService smsService, ILogger<SendBulkSmsHandler> logger)
    {
        _smsService = smsService;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(SendBulkSmsCommand request, CancellationToken ct)
    {
        var result = await _smsService.SendBulkSmsAsync(request.BulkSms.PhoneNumbers, request.BulkSms.Message);
        _logger.LogInformation("Bulk SMS sent to {Count} recipients: {Result}", request.BulkSms.PhoneNumbers.Count, result);
        return result
            ? ApiResponse<bool>.Success(true, "Bulk SMS sent successfully")
            : ApiResponse<bool>.Failure("Failed to send bulk SMS");
    }
}

// ── Payment Commands ──────────────────────────────────────────────────

public record InitiatePaymentCommand(PaymentRequestDto Payment) : IRequest<ApiResponse<PaymentResultDto>>;

public class InitiatePaymentHandler : IRequestHandler<InitiatePaymentCommand, ApiResponse<PaymentResultDto>>
{
    private readonly IPaymentGatewayService _paymentService;

    public InitiatePaymentHandler(IPaymentGatewayService paymentService) => _paymentService = paymentService;

    public async Task<ApiResponse<PaymentResultDto>> Handle(InitiatePaymentCommand request, CancellationToken ct)
    {
        var paymentRequest = new PaymentRequest
        {
            Amount = request.Payment.Amount,
            Currency = request.Payment.Currency,
            ReferenceId = request.Payment.ReferenceId,
            CustomerName = request.Payment.CustomerName,
            ReturnUrl = request.Payment.ReturnUrl
        };

        var result = await _paymentService.InitiatePaymentAsync(paymentRequest);
        var dto = new PaymentResultDto
        {
            IsSuccess = result.IsSuccess,
            TransactionId = result.TransactionId,
            RedirectUrl = result.RedirectUrl,
            ErrorMessage = result.ErrorMessage
        };

        return result.IsSuccess
            ? ApiResponse<PaymentResultDto>.Success(dto, "Payment initiated")
            : ApiResponse<PaymentResultDto>.Failure(result.ErrorMessage ?? "Payment initiation failed");
    }
}

public record VerifyPaymentCommand(PaymentVerificationDto Verification) : IRequest<ApiResponse<PaymentResultDto>>;

public class VerifyPaymentHandler : IRequestHandler<VerifyPaymentCommand, ApiResponse<PaymentResultDto>>
{
    private readonly IPaymentGatewayService _paymentService;

    public VerifyPaymentHandler(IPaymentGatewayService paymentService) => _paymentService = paymentService;

    public async Task<ApiResponse<PaymentResultDto>> Handle(VerifyPaymentCommand request, CancellationToken ct)
    {
        var result = await _paymentService.VerifyPaymentAsync(request.Verification.TransactionId);
        var dto = new PaymentResultDto
        {
            IsSuccess = result.IsSuccess,
            TransactionId = result.TransactionId,
            ErrorMessage = result.ErrorMessage
        };

        return result.IsSuccess
            ? ApiResponse<PaymentResultDto>.Success(dto, "Payment verified")
            : ApiResponse<PaymentResultDto>.Failure(result.ErrorMessage ?? "Payment verification failed");
    }
}

// ── SSF Commands ──────────────────────────────────────────────────────

public record RegisterSSFEmployeeCommand(SSFRegistrationRequestDto Registration) : IRequest<ApiResponse<SSFRegistrationResultDto>>;

public class RegisterSSFEmployeeHandler : IRequestHandler<RegisterSSFEmployeeCommand, ApiResponse<SSFRegistrationResultDto>>
{
    private readonly ISSFApiService _ssfService;

    public RegisterSSFEmployeeHandler(ISSFApiService ssfService) => _ssfService = ssfService;

    public async Task<ApiResponse<SSFRegistrationResultDto>> Handle(RegisterSSFEmployeeCommand request, CancellationToken ct)
    {
        var dto = new SSFEmployeeDto
        {
            EmployeeName = request.Registration.EmployeeName,
            PanNumber = request.Registration.PanNumber,
            DateOfBirth = request.Registration.DateOfBirth,
            JoinDate = request.Registration.JoinDate,
            Gender = request.Registration.Gender,
            BasicSalary = request.Registration.BasicSalary
        };

        var result = await _ssfService.RegisterEmployeeAsync(dto);
        var resultDto = new SSFRegistrationResultDto
        {
            IsSuccess = result.IsSuccess,
            SSFNumber = result.SSFNumber,
            ErrorMessage = result.ErrorMessage
        };

        return result.IsSuccess
            ? ApiResponse<SSFRegistrationResultDto>.Success(resultDto, "SSF registration successful")
            : ApiResponse<SSFRegistrationResultDto>.Failure(result.ErrorMessage ?? "SSF registration failed");
    }
}

public record SubmitSSFContributionCommand(SSFContributionRequestDto Contribution) : IRequest<ApiResponse<SSFContributionResultDto>>;

public class SubmitSSFContributionHandler : IRequestHandler<SubmitSSFContributionCommand, ApiResponse<SSFContributionResultDto>>
{
    private readonly ISSFApiService _ssfService;

    public SubmitSSFContributionHandler(ISSFApiService ssfService) => _ssfService = ssfService;

    public async Task<ApiResponse<SSFContributionResultDto>> Handle(SubmitSSFContributionCommand request, CancellationToken ct)
    {
        var dto = new SSFContributionDto
        {
            SSFNumber = request.Contribution.SSFNumber,
            EmployeeContribution = request.Contribution.EmployeeContribution,
            EmployerContribution = request.Contribution.EmployerContribution,
            ContributionMonth = request.Contribution.ContributionMonth,
            ContributionYear = request.Contribution.ContributionYear
        };

        var result = await _ssfService.SubmitContributionAsync(dto);
        var resultDto = new SSFContributionResultDto
        {
            IsSuccess = result.IsSuccess,
            ReferenceNumber = result.ReferenceNumber,
            ErrorMessage = result.ErrorMessage
        };

        return result.IsSuccess
            ? ApiResponse<SSFContributionResultDto>.Success(resultDto, "SSF contribution submitted")
            : ApiResponse<SSFContributionResultDto>.Failure(result.ErrorMessage ?? "SSF contribution failed");
    }
}

// ── Import/Export Commands ────────────────────────────────────────────

public record ImportLedgersCommand(Stream FileStream) : IRequest<ApiResponse<ImportResultDto>>;

public class ImportLedgersHandler : IRequestHandler<ImportLedgersCommand, ApiResponse<ImportResultDto>>
{
    private readonly IExcelService _excelService;
    private readonly IApplicationDbContext _db;

    public ImportLedgersHandler(IExcelService excelService, IApplicationDbContext db)
    {
        _excelService = excelService;
        _db = db;
    }

    public async Task<ApiResponse<ImportResultDto>> Handle(ImportLedgersCommand request, CancellationToken ct)
    {
        var result = new ImportResultDto();
        try
        {
            var ledgers = await _excelService.ImportFromExcelAsync<Domain.Entities.Account.Ledger>(request.FileStream);
            result.TotalRows = ledgers.Count;

            foreach (var ledger in ledgers)
            {
                try
                {
                    _db.Ledgers.Add(ledger);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Row error: {ex.Message}");
                }
            }

            await _db.SaveChangesAsync(ct);
            return ApiResponse<ImportResultDto>.Success(result, $"Imported {result.SuccessCount} of {result.TotalRows} ledgers");
        }
        catch (Exception ex)
        {
            return ApiResponse<ImportResultDto>.Failure($"Import failed: {ex.Message}");
        }
    }
}

public record ImportProductsCommand(Stream FileStream) : IRequest<ApiResponse<ImportResultDto>>;

public class ImportProductsHandler : IRequestHandler<ImportProductsCommand, ApiResponse<ImportResultDto>>
{
    private readonly IExcelService _excelService;
    private readonly IApplicationDbContext _db;

    public ImportProductsHandler(IExcelService excelService, IApplicationDbContext db)
    {
        _excelService = excelService;
        _db = db;
    }

    public async Task<ApiResponse<ImportResultDto>> Handle(ImportProductsCommand request, CancellationToken ct)
    {
        var result = new ImportResultDto();
        try
        {
            var products = await _excelService.ImportFromExcelAsync<Domain.Entities.Inventory.Product>(request.FileStream);
            result.TotalRows = products.Count;

            foreach (var product in products)
            {
                try
                {
                    _db.Products.Add(product);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Row error: {ex.Message}");
                }
            }

            await _db.SaveChangesAsync(ct);
            return ApiResponse<ImportResultDto>.Success(result, $"Imported {result.SuccessCount} of {result.TotalRows} products");
        }
        catch (Exception ex)
        {
            return ApiResponse<ImportResultDto>.Failure($"Import failed: {ex.Message}");
        }
    }
}
