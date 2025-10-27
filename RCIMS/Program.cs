using System.IO.Compression;
using System.Net;
using Interface;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using NLog;
using NLog.Web;
using RCIMS.Extensions;
using RCIMS.Migrations;
using Repository;
using Shared.Helpers;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();

    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "application/problem+json",
        "text/plain"
    });

    options.EnableForHttps = true;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
LogManager.Setup()
    .LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Logging.ClearProviders();
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();
//todo: for notifications
//builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
builder.Services.ConfigureFluentMigrator(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureOpenApi();
builder.Services.AddAuthentication();
builder.Services.ConfigureRateLimiting();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateConverter());
    });
builder.Host.UseNLog();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();




var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
var cfgFwd = builder.Configuration.GetSection("ForwardedHeaders");
var fwd = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor
                       | ForwardedHeaders.XForwardedProto
                       | ForwardedHeaders.XForwardedHost,
    ForwardLimit = cfgFwd.GetValue("ForwardLimit", 2),
    RequireHeaderSymmetry = cfgFwd.GetValue("RequireHeaderSymmetry", false)
};

foreach (var cidr in cfgFwd.GetSection("KnownNetworks").Get<string[]>() ?? Array.Empty<string>())
{
    var parts = cidr.Split('/');
    fwd.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse(parts[0]), int.Parse(parts[1])));
}
foreach (var ip in cfgFwd.GetSection("KnownProxies").Get<string[]>() ?? Array.Empty<string>())
    fwd.KnownProxies.Add(IPAddress.Parse(ip));

app.UseForwardedHeaders(fwd);
app.UseCors("CorsPolicy");
app.MapOpenApi();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/openapi/v1.json", "RCIMS API V1");
    s.RoutePrefix = string.Empty;
});
app.UseAuthentication();
app.UseRateLimiter();
app.UseMiddleware<ExpiredOrMissedTokenMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase(logger);
app.Run();


