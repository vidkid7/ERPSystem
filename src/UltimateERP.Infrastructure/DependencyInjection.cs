using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateERP.Application.Interfaces;
using UltimateERP.Application.Services;
using UltimateERP.Domain.Interfaces;
using UltimateERP.Infrastructure.Auth;
using UltimateERP.Infrastructure.Caching;
using UltimateERP.Infrastructure.ExternalServices;
using UltimateERP.Infrastructure.Persistence;
using UltimateERP.Infrastructure.Persistence.Interceptors;
using UltimateERP.Infrastructure.Repositories;
using UltimateERP.Infrastructure.Services;

namespace UltimateERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<ERPDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ERPDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                })
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ERPDbContext>());
        services.AddSingleton<IDateTimeService, DateTimeService>();

        // Caching
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        // Audit logging
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Nepal IRD integration
        services.AddHttpClient<IIRDApiClient, IRDApiClient>(client =>
        {
            var irdBaseUrl = configuration.GetValue<string>("IRD:BaseUrl") ?? "https://cbms.ird.gov.np/";
            client.BaseAddress = new Uri(irdBaseUrl);
        });

        // Lab workflow
        services.AddScoped<LabWorkflowService>();

        // Auth services
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        // External integrations (Task 30)
        services.AddHttpClient<ISmsService, SmsService>();
        services.AddHttpClient<IPaymentGatewayService, FonePayService>();
        services.AddHttpClient<IPushNotificationService, OneSignalService>();
        services.AddHttpClient<ISSFApiService, SSFApiService>();

        // Data import/export (Task 31)
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<ITallyIntegrationService, TallyIntegrationService>();

        return services;
    }
}
