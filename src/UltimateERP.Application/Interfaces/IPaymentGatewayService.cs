namespace UltimateERP.Application.Interfaces;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "NPR";
    public string? ReferenceId { get; set; }
    public string? CustomerName { get; set; }
    public string? ReturnUrl { get; set; }
}

public class PaymentInitiationResult
{
    public bool IsSuccess { get; set; }
    public string? TransactionId { get; set; }
    public string? RedirectUrl { get; set; }
    public string? ErrorMessage { get; set; }
}

public class PaymentVerificationResult
{
    public bool IsSuccess { get; set; }
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface IPaymentGatewayService
{
    Task<PaymentInitiationResult> InitiatePaymentAsync(PaymentRequest request);
    Task<PaymentVerificationResult> VerifyPaymentAsync(string transactionId);
}
