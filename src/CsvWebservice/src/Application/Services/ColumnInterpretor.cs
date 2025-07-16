using Bison.CsvWebservice.Application.Abstractions;
using Bison.CsvWebservice.Application.Common.Models;
using Bison.CsvWebservice.Application.Options;
using Bison.CsvWebservice.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bison.CsvWebservice.Application.Services;

public class ColumnInterpretor : IColumnInterpretor
{
	private readonly IMemoryCache _cache;
	private readonly HttpClient _httpClient;
	private readonly CachingOptions _cachingOptions;

	public ColumnInterpretor(IMemoryCache cache, HttpClient httpClient, IOptions<CachingOptions> cachingOptions)
	{
		_cache = cache;
		_httpClient = httpClient;
		_cachingOptions = cachingOptions.Value;
	}

	public async Task<CsvColumn> GetValueAsync(string value)
	{
		CsvColumn result;

		//Leverage caching to avoid unnecessary HTTP requests
		User cachedUser = await _cache.GetOrCreateAsync(value, async entry =>
		{
			entry.SetSlidingExpiration(TimeSpan.FromMinutes(_cachingOptions.CacheDurationInMinutes));
			var response = await _httpClient.GetAsync($"api/users/{value}");
			if (response.IsSuccessStatusCode)
			{
				User user = await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				return user;
			}
			else
			{
				// If the user is not found, we set the cache entry to expire immediately
				entry.AbsoluteExpiration = DateTimeOffset.MinValue;
				return null;
			}
		});
		if (cachedUser is null)
		{
			result = new CsvColumn(value, CsvColumnType.PlainString);
		}
		else
		{
			result = new CsvColumn(cachedUser.Email, CsvColumnType.ParsedEmail);
		}

		return result;

	}


}
