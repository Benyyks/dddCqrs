
using Bison.CsvWebservice.Application.Common.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Bison.CsvWebservice.Web
{
	public class CachePreloadService : IHostedService
	{
		private readonly IMemoryCache _cache;
		private readonly HttpClient _httpClient;
		private readonly ILogger<CachePreloadService> _logger;

		public CachePreloadService(IMemoryCache memoryCache, HttpClient httpClient, ILogger<CachePreloadService> logger)
		{
			_cache = memoryCache;
			_httpClient = httpClient;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/users");
				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError("Failed to preload cache from User Webservice. Status code: {StatusCode}", response.StatusCode);
					throw new InvalidOperationException("Failed to preload cache from User Webservice.");
				}
				IEnumerable<User> user = await JsonSerializer.DeserializeAsync<IEnumerable<User>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				foreach (User u in user)
				{
					_cache.Set(u.Id.ToString(), u, new MemoryCacheEntryOptions()
					{
						SlidingExpiration = TimeSpan.FromMinutes(360) // Set cache expiration to 6 hours
					});
				}
				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while preloading the cache from User Webservice.");
			}
			finally
			{
				StartupGateMiddleware.SetReady();
			}
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}


}
