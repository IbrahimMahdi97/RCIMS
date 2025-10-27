using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using FluentMigrator.Runner;
using Interface;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service;
using Service.Interface;

namespace RCIMS.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureFluentMigrator(this IServiceCollection services,
        IConfiguration configuration) => services.AddLogging(c =>
            c.AddFluentMigratorConsole())
                .AddFluentMigratorCore().ConfigureRunner(c =>
                    c.AddPostgres()
                        .WithGlobalConnectionString(configuration
                            .GetConnectionString("postgresConnection"))
                        .ScanIn(Assembly.GetExecutingAssembly())
                            .For.Migrations());

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["secretKey"];

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });
    }

    public static void ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
    }
    
    public static void ConfigureRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = (ctx, _) =>
            {
                if (ctx.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    ctx.HttpContext.Response.Headers["Retry-After"] =
                        Math.Ceiling(retryAfter.TotalSeconds).ToString();
                }
                return ValueTask.CompletedTask;
            };

            string GetClientKey(HttpContext ctx)
            {
                var uid = ctx.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? ctx.User?.FindFirst("sub")?.Value;

                return uid ?? ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            }

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            {
                var key = GetClientKey(ctx);
                return RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 6,
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
            options.AddPolicy("signin", httpContext =>
            {
                var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "ip-unknown";
                return RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 1,
                    QueueLimit = 1,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });

            // Concurrency limiter for heavy endpoints
            options.AddConcurrencyLimiter("heavy", o =>
            {
                o.PermitLimit = 100;
                o.QueueLimit = 500;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });
    }
}