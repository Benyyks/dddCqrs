using FluentAssertions;
using Bison.CsvWebservice.Application.Abstractions;
using Bison.CsvWebservice.Application.Handlers.Commands;
using Bison.CsvWebservice.Application.Handlers.Models;
using Bison.CsvWebservice.Application.Options;
using Bison.CsvWebservice.Application.Resources;
using Bison.CsvWebservice.Application.Services;
using Bison.CsvWebservice.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;

namespace Bison.CsvWebService.Application.Tests
{
	internal class CsvUploadHandlerTests
	{
		private CsvParseHandler _handler;
		private ICsvParser _csvParser;
		private IColumnInterpretor _columnInterpretor;
		private Mock<IOptions<CsvParserOptions>> _csvParserOptionsMock;
		private Mock<IOptions<CachingOptions>> _cachingOptionsMock;
		private IMemoryCache _memoryCache;

		[SetUp]
		public void Setup()
		{
			_memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
			_csvParserOptionsMock = new Mock<IOptions<CsvParserOptions>>();
			_cachingOptionsMock = new Mock<IOptions<CachingOptions>>();
		}

		[TearDown]
		public void TearDown()
		{
			_memoryCache?.Dispose();
		}

		[Test]
		public async Task GenerateReportWithCachePreloadAsync()
		{
			var client = new HttpClient(new HttpMessageHandlerGetByIdMock()) { BaseAddress = new Uri("http://mock") };
			_cachingOptionsMock.Setup(x => x.Value).Returns(new CachingOptions { UseCachePreload = true });
			_csvParserOptionsMock.Setup(x => x.Value).Returns(new CsvParserOptions());
			_columnInterpretor = new ColumnInterpretor(_memoryCache, client, _cachingOptionsMock.Object);
			_csvParser = new CsvParser(_columnInterpretor, _csvParserOptionsMock.Object, new Mock<ILogger<CsvParser>>().Object);
			_handler = new CsvParseHandler(_csvParser, new Mock<ILogger<CsvParseHandler>>().Object);

			using var fs = File.OpenRead("data/input.csv");

			CsvParsedReport csvParsedReport = await _handler.Handle(new CsvParseCommand()
			{
				Content = fs,
				ContentLength = fs.Length
			}, CancellationToken.None);
			csvParsedReport.Total.Should().Be(3);
			csvParsedReport.Found.Should().Be(3);
			foreach(var line in csvParsedReport.ValueLines)
			{
				line.Values.Count.Should().Be(4); // 3 columns + Status
				line.Should().ContainKey("Status");
				line["Status"].Should().Be(CsvLineStatus.Found.ToString());
			}
		}

		[Test]
		public async Task GenerateReportWithEmptyContentAsync()
		{
			var client = new HttpClient(new HttpMessageHandlerGetByIdMock()) { BaseAddress = new Uri("http://mock") };
			_cachingOptionsMock.Setup(x => x.Value).Returns(new CachingOptions { UseCachePreload = true });
			_csvParserOptionsMock.Setup(x => x.Value).Returns(new CsvParserOptions());
			_columnInterpretor = new ColumnInterpretor(_memoryCache, client, _cachingOptionsMock.Object);
			_csvParser = new CsvParser(_columnInterpretor, _csvParserOptionsMock.Object, new Mock<ILogger<CsvParser>>().Object);
			_handler = new CsvParseHandler(_csvParser, new Mock<ILogger<CsvParseHandler>>().Object);

			using var fs = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

			await _handler.Invoking(async x => await x.Handle(new CsvParseCommand()
			{
				Content = fs,
				ContentLength = fs.Length
			}, CancellationToken.None))
				.Should().ThrowAsync<ArgumentException>()
				.WithMessage(DefaultResources.CsvEmptyErrorMessage);
		}

		[Test]
		public async Task GenerateReportWithWrongConfigurationAsync()
		{
			const string columnId = "Some Id";
			var client = new HttpClient(new HttpMessageHandlerGetByIdMock()) { BaseAddress = new Uri("http://mock") };
			_cachingOptionsMock.Setup(x => x.Value).Returns(new CachingOptions { UseCachePreload = true });
			_csvParserOptionsMock.Setup(x => x.Value).Returns(new CsvParserOptions()
			{
				ColumnId = columnId,
				Delimiter = ','
			});
			_columnInterpretor = new ColumnInterpretor(_memoryCache, client, _cachingOptionsMock.Object);
			_csvParser = new CsvParser(_columnInterpretor, _csvParserOptionsMock.Object, new Mock<ILogger<CsvParser>>().Object);
			_handler = new CsvParseHandler(_csvParser, new Mock<ILogger<CsvParseHandler>>().Object);

			using var fs = File.OpenRead("data/input.csv");

			await _handler.Invoking(async x => await x.Handle(new CsvParseCommand()
			{
				Content = fs,
				ContentLength = fs.Length
			}, CancellationToken.None))
				.Should().ThrowAsync<InvalidOperationException>()
				.WithMessage(string.Format(DefaultResources.ColumnIdNotFoundErrorMessage, columnId));
		}
	}
}
