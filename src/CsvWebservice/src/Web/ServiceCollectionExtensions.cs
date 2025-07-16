using Bison.CsvWebservice.Web.Options;
using System.Threading.RateLimiting;

namespace Bison.CsvWebservice.Web
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection ConfigureRateLimiting(this IServiceCollection services, RateLimitOptions options)
		{
			return services.AddRateLimiter(rateLimiterOptions =>
			{
				rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
				rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => {
					string clientIp = httpContext.Request.Headers["X-Forwarded-For"].ToString();
					if (string.IsNullOrWhiteSpace(clientIp))
						clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
					return RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: clientIp,
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = options.PermitLimit,
							Window = TimeSpan.FromMinutes(options.WindowInMinutes),
							QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
							QueueLimit = 0
						});
				});
			});
		}
	}
}
