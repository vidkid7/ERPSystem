using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.ExternalServices;

public class FonePayService : IPaymentGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FonePayService> _logger;
    private readonly string _merchantCode;
    private readonly string _secretKey;
    private readonly string _baseUrl;

    public FonePayService(HttpClient httpClient, ILogger<FonePayService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _merchantCode = configuration["FonePay:MerchantCode"] ?? string.Empty;
        _secretKey = configuration["FonePay:SecretKey"] ?? string.Empty;
        _baseUrl = configuration["FonePay:BaseUrl"] ?? "https://fonepay-api.example.com";
    }

    public async Task<PaymentInitiationResult> InitiatePaymentAsync(PaymentRequest request)
    {
        try
        {
            _logger.LogInformation("Initiating FonePay payment for amount {Amount} {Currency}", request.Amount, request.Currency);

            var payload = new
            {
                merchantCode = _merchantCode,
                amount = request.Amount,
                currency = request.Currency,
                referenceId = request.ReferenceId ?? Guid.NewGuid().ToString(),
                customerName = request.CustomerName,
                returnUrl = request.ReturnUrl
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/initiate", payload);

            if (response.IsSuccessStatusCode)
            {
                var transactionId = Guid.NewGuid().ToString("N")[..12].ToUpper();
                _logger.LogInformation("FonePay payment initiated: {TransactionId}", transactionId);

                return new PaymentInitiationResult
                {
                    IsSuccess = true,
                    TransactionId = transactionId,
                    RedirectUrl = $"{_baseUrl}/pay/{transactionId}"
                };
            }

            return new PaymentInitiationResult
            {
                IsSuccess = false,
                ErrorMessage = "Payment initiation failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FonePay payment initiation error");
            return new PaymentInitiationResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentVerificationResult> VerifyPaymentAsync(string transactionId)
    {
        try
        {
            _logger.LogInformation("Verifying FonePay payment: {TransactionId}", transactionId);

            var response = await _httpClient.GetAsync($"{_baseUrl}/verify/{transactionId}?merchantCode={_merchantCode}");

            if (response.IsSuccessStatusCode)
            {
                return new PaymentVerificationResult
                {
                    IsSuccess = true,
                    TransactionId = transactionId,
                    Status = "Completed"
                };
            }

            return new PaymentVerificationResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                ErrorMessage = "Verification failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FonePay verification error for {TransactionId}", transactionId);
            return new PaymentVerificationResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                ErrorMessage = ex.Message
            };
        }
    }
}
