namespace UltimateERP.Application.Common.Exceptions;

/// <summary>
/// Thrown when input validation fails (distinct from FluentValidation's ValidationException).
/// </summary>
public class AppValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public AppValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public AppValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public AppValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}
