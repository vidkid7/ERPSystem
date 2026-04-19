namespace UltimateERP.Application.Common.Exceptions;

/// <summary>
/// Thrown when the current user does not have permission for the requested action.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("You do not have permission to perform this action.") { }

    public ForbiddenAccessException(string message) : base(message) { }
}
