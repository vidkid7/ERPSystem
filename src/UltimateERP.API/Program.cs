using Serilog;
using Microsoft.OpenApi;
using UltimateERP.Application;
using UltimateERP.Infrastructure;
using UltimateERP.Domain.Interfaces;
using UltimateERP.API.Common;
using UltimateERP.API.Middleware;
using UltimateERP.API.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ──────────────────────────────────────────────────────────────
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console();

    var elasticUri = context.Configuration["ElasticSearch:Uri"];
    if (!string.IsNullOrWhiteSpace(elasticUri))
    {
        loggerConfig.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"ultimateerp-logs-{context.HostingEnvironment.EnvironmentName?.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
        });
    }
});

// ── Services ─────────────────────────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ultimate ERP API",
        Version = "v1",
        Description = "Comprehensive Enterprise Resource Planning API — 16+ modules, 426 entities, 97 endpoints"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// ── CORS ─────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:3000"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ── Health Checks ────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<SystemHealthCheck>("system");

var app = builder.Build();

// ── Middleware Pipeline ──────────────────────────────────────────────────
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ultimate ERP API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Name == "database",
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            Status = report.Status.ToString(),
            Timestamp = DateTime.UtcNow,
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description
            })
        });
    }
}).WithTags("System");

app.MapHealthChecks("/health/system", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Name == "system",
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            Status = report.Status.ToString(),
            Timestamp = DateTime.UtcNow,
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description,
                Data = e.Value.Data
            })
        });
    }
}).WithTags("System");

app.Run();
