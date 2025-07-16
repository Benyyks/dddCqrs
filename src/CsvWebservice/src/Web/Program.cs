using Bison.CsvWebservice.Application;
using Bison.CsvWebservice.Application.Options;
using Bison.CsvWebservice.Web;
using Bison.CsvWebservice.Web.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
bool isKubernetes = Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST") != null;

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile($"{(isKubernetes ? "config/" : "")}appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"{(isKubernetes ? "config/" : "")}appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

var configurationRoot = configurationBuilder.Build();
bool usePreloadCache = configurationRoot.GetSection("CacheSettings:UseCachePreload").Get<bool>();
Uri userWebServiceUrl = configurationRoot.GetSection("UserWebServiceUrl").Get<Uri>();
RateLimitOptions rateLimitOptions = configurationRoot.GetSection("RateLimiting").Get<RateLimitOptions>();

builder.Services.AddControllers();
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.AllowSynchronousIO = true;
});

builder.Services.AddHealthChecks();
builder.Services.AddApplicationServices(userWebServiceUrl);
builder.Services.AddMemoryCache();
builder.Services.Configure<CachingOptions>(configurationRoot.GetSection("CacheSettings"));
builder.Services.Configure<CsvParserOptions>(configurationRoot.GetSection("CsvParser"));
if (usePreloadCache)
{
	builder.Services.AddHttpClient<IHostedService, CachePreloadService>(client =>
	{
		client.BaseAddress = userWebServiceUrl;
	})
	.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
		.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(2)));
}
builder.Services.ConfigureRateLimiting(rateLimitOptions);

var app = builder.Build();
app.UseHttpsRedirection();
if (usePreloadCache)
{
	app.UseMiddleware<StartupGateMiddleware>();
}
if(rateLimitOptions.Enabled) 
{ 
	app.UseRateLimiter();
}
app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
