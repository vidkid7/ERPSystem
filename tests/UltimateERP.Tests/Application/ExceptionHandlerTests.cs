using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using UltimateERP.API.Middleware;
using UltimateERP.Application.Common.Exceptions;

namespace UltimateERP.Tests.Application;

public class ExceptionHandlerTests
{
    private readonly Mock<ILogger<GlobalExceptionHandlerMiddleware>> _loggerMock = new();

    private GlobalExceptionHandlerMiddleware CreateMiddleware(RequestDelegate next)
    {
        return new GlobalExceptionHandlerMiddleware(next, _loggerMock.Object);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    [Fact]
    public async Task NotFoundException_Returns404()
    {
        var middleware = CreateMiddleware(_ => throw new NotFoundException("Product", 123));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
    }

    [Fact]
    public async Task ForbiddenAccessException_Returns403()
    {
        var middleware = CreateMiddleware(_ => throw new ForbiddenAccessException());
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task ConflictException_Returns409()
    {
        var middleware = CreateMiddleware(_ => throw new ConflictException("Duplicate code"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.Conflict, context.Response.StatusCode);
    }

    [Fact]
    public async Task BusinessRuleException_Returns422()
    {
        var middleware = CreateMiddleware(_ => throw new BusinessRuleException("InsufficientStock", "Insufficient stock"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal(422, context.Response.StatusCode);
    }

    [Fact]
    public async Task AppValidationException_Returns400()
    {
        var middleware = CreateMiddleware(_ => throw new AppValidationException("Invalid input"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task FluentValidationException_Returns400()
    {
        var failures = new List<ValidationFailure> { new("Field", "Field is required") };
        var middleware = CreateMiddleware(_ => throw new ValidationException(failures));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task UnhandledException_Returns500()
    {
        var middleware = CreateMiddleware(_ => throw new InvalidOperationException("Unexpected"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task NoException_PassesThrough()
    {
        var middleware = CreateMiddleware(ctx =>
        {
            ctx.Response.StatusCode = 200;
            return Task.CompletedTask;
        });
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal(200, context.Response.StatusCode);
    }
}
