using MediatR;
using Microsoft.Extensions.Logging;

namespace UltimateERP.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs unhandled exceptions.
/// </summary>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "UltimateERP Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
            throw;
        }
    }
}
