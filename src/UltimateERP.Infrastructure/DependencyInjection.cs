using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Interfaces;
using UltimateERP.Infrastructure.Auth;
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

        // Auth services
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        return services;
    }
}
