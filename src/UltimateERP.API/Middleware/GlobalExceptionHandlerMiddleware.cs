using System.Net;
using System.Text.Json;
using FluentValidation;
using UltimateERP.Application.Common.Exceptions;
using UltimateERP.Application.Common.Models;

namespace UltimateERP.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
            var response = ApiResponse.Failure($"Validation failed: {errors}");
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (AppValidationException ex)
        {
            _logger.LogWarning(ex, "Application validation error occurred");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ForbiddenAccessException ex)
        {
            _logger.LogWarning(ex, "Forbidden access attempt");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure("Access denied");
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "Conflict detected");
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (BusinessRuleException ex)
        {
            _logger.LogWarning(ex, "Business rule violation: {RuleName}", ex.RuleName);
            context.Response.StatusCode = 422; // Unprocessable Entity
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Failure("An internal server error occurred. Please try again later.");
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
