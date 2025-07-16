using CsvWebService.Application.Tests;
using Bison.CsvWebservice.Application.Common.Models;
using System.Text.Json;

namespace Bison.CsvWebService.Application.Tests
{
	public class HttpMessageHandlerGetAllMock : HttpMessageHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			using var fs = File.OpenRead("data/mockdb.json");
			using var streamReader = new StreamReader(fs);
			string mockResponse = await streamReader.ReadToEndAsync();

			return new HttpResponseMessage()
			{
				Content = new StringContent(mockResponse)
			};
		}
	}

	public class HttpMessageHandlerGetByIdMock : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			string[] segments = request.RequestUri.AbsolutePath.Split('/');
			User user = Helper.GenerateUser(Guid.Parse(segments.Last()));
			string mockResponse = JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return Task.FromResult(new HttpResponseMessage()
			{
				Content = new StringContent(mockResponse)
			});
		}
	}
}
