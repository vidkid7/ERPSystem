using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Common.Validators;

namespace UltimateERP.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that sanitizes string properties on incoming requests.
/// </summary>
public class SanitizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<SanitizationBehavior<TRequest, TResponse>> _logger;

    public SanitizationBehavior(ILogger<SanitizationBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        SanitizeProperties(request);
        return await next(cancellationToken);
    }

    private void SanitizeProperties(TRequest request)
    {
        var stringProperties = typeof(TRequest)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

        foreach (var property in stringProperties)
        {
            var value = property.GetValue(request) as string;
            if (value != null && InputSanitizer.ContainsDangerousContent(value))
            {
                _logger.LogWarning("Sanitized potentially dangerous input in {Property} of {Request}",
                    property.Name, typeof(TRequest).Name);
                property.SetValue(request, InputSanitizer.Sanitize(value));
            }
        }
    }
}
