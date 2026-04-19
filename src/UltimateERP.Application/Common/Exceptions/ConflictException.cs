namespace UltimateERP.Application.Common.Exceptions;

/// <summary>
/// Thrown when an operation conflicts with existing data (e.g., duplicate code).
/// </summary>
public class ConflictException : Exception
{
    public ConflictException() : base() { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key ({key}) already exists.") { }
}
