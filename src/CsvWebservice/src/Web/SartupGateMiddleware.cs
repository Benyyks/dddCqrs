namespace Bison.CsvWebservice.Web
{
	public class StartupGateMiddleware
	{
		private static volatile bool _isReady = false;

		public static void SetReady() => _isReady = true;

		private readonly RequestDelegate _next;

		public StartupGateMiddleware(RequestDelegate next) => _next = next;

		public async Task Invoke(HttpContext context)
		{
			if (!_isReady)
			{
				context.Response.StatusCode = 503; // Service Unavailable
				await context.Response.WriteAsync("Service is warming up.");
				return;
			}

			await _next(context);
		}
	}

}
