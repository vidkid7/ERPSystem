namespace UltimateERP.Application.Common.Exceptions;

/// <summary>
/// Thrown when a business rule is violated (e.g., insufficient stock, fiscal year closed).
/// </summary>
public class BusinessRuleException : Exception
{
    public string RuleName { get; }

    public BusinessRuleException(string message) : base(message)
    {
        RuleName = string.Empty;
    }

    public BusinessRuleException(string ruleName, string message) : base(message)
    {
        RuleName = ruleName;
    }
}
