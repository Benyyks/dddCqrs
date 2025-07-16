using Bison.CsvWebservice.Application.Abstractions;
using Bison.CsvWebservice.Application.Handlers.Models;
using Bison.CsvWebservice.Application.Resources;
using Bison.CsvWebservice.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bison.CsvWebservice.Application.Handlers.Commands
{
	public class CsvParseHandler : IRequestHandler<CsvParseCommand, CsvParsedReport>
	{
		private readonly ICsvParser _csvParser;
		private readonly ILogger<CsvParseHandler> _logger;
		private const string StatusKey = "Status";

		public CsvParseHandler(ICsvParser csvParser, ILogger<CsvParseHandler> logger)
		{
			_csvParser = csvParser;
			_logger = logger;
		}

		public async Task<CsvParsedReport> Handle(CsvParseCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request.ContentLength == 0)
				{
					throw new ArgumentException(DefaultResources.CsvEmptyErrorMessage);
				}

				CsvParsedFile csv = await _csvParser.ParseCsv(request.Content);

				CsvParsedReport csvParsedReport = new CsvParsedReport();
				csvParsedReport.Total = csv.Lines.Count;
				csvParsedReport.Errors = csv.Lines.Count(x => x.Status == CsvLineStatus.Error);
				csvParsedReport.Found = csv.Lines.Where(x => x.Status == CsvLineStatus.Found).Count();

				var valueLines = new List<Dictionary<string, string>>();
				int index = 0;
				csvParsedReport.ValueLines = csv.Lines.Aggregate(valueLines, (acc, line) =>
				{
					var lineResults = csv.RenderLine(index++);
					lineResults.Add(StatusKey, line.Status.ToString()); //keep track of the status of the line
					acc.Add(lineResults);
					return acc;
				});
				return csvParsedReport;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}
	}
}
