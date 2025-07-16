using Bison.CsvWebservice.Application.Abstractions;
using Bison.CsvWebservice.Application.Options;
using Bison.CsvWebservice.Application.Resources;
using Bison.CsvWebservice.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace Bison.CsvWebservice.Application.Services;

public class CsvParser : ICsvParser
{
	private readonly IColumnInterpretor _columnInterpretor;
	private readonly CsvParserOptions _options;
	private readonly ILogger<CsvParser> _logger;

	public CsvParser(IColumnInterpretor columnInterpretor, IOptions<CsvParserOptions> options, ILogger<CsvParser> logger)
	{
		_columnInterpretor = columnInterpretor;
		_options = options.Value;
		_logger = logger;
	}

	public async Task<CsvParsedFile> ParseCsv(Stream csvContent, int batchSize = 100)
	{
		CsvParsedFile result = new CsvParsedFile();
		string content = string.Empty;
		using (StreamReader reader = new StreamReader(csvContent, Encoding.UTF8))
		{
			//First line is the header
			string header = await reader.ReadLineAsync();
			result.SetHeader(header, _options.Delimiter);

			int userIdIndex = GetUserIdIndex(header);
			if(userIdIndex == -1)
				throw new InvalidOperationException(string.Format(DefaultResources.ColumnIdNotFoundErrorMessage, _options.ColumnId));

			var batch = new List<string>(batchSize);
			string line = string.Empty;
			while ((line = await reader.ReadLineAsync()) != null)
			{
				//skip blank lines
				if (string.IsNullOrWhiteSpace(line))
					continue;

				batch.Add(line);
				if (batch.Count == batchSize)
				{
					var processedLines = await ProcessLinesAsync(batch, userIdIndex);
					result.AddLines(processedLines);
					batch.Clear();
				}
			}

			// Process any remaining lines in the batch
			if (batch.Count > 0)
			{
				var processedLines = await ProcessLinesAsync(batch, userIdIndex);
				result.AddLines(processedLines);
				batch.Clear();
			}
		}

		return result;
	}

	private int GetUserIdIndex(string header)
	{
		string[] headers = header.Split(_options.Delimiter);
		int index = Array.FindIndex(headers, h => string.Equals(h, _options.ColumnId, StringComparison.OrdinalIgnoreCase));
		return index;
	}
		

	private async Task<List<CsvLine>> ProcessLinesAsync(List<string> batch, int userIdIndex)
	{
		var tasks = batch.Select(x => ProcessLineAsync(x, userIdIndex));
		// Use Task.WhenAll to process all lines concurrently
		var lines = await Task.WhenAll(tasks);
		return lines.ToList();
	}

	private async Task<CsvLine> ProcessLineAsync(string line, int userIdIndex)
	{
		var columns = line.Split(',');
		var csvColumns = new List<CsvColumn>();
		int index = 0;
		CsvLineStatus parsed = CsvLineStatus.NotFound;
		foreach (var column in columns)
		{
			//Effectively perform Http request only for the userId column
			if (index == userIdIndex)
			{
				try
				{
					CsvColumn parsedColumn = await _columnInterpretor.GetValueAsync(column);
					csvColumns.Add(parsedColumn);
					parsed = parsedColumn.Parsed ? CsvLineStatus.Found : CsvLineStatus.NotFound;
				}
				catch(Exception e)
				{
					parsed = CsvLineStatus.Error;
					_logger.LogError(e, e.Message);
				}
			}
			else
			{
				csvColumns.Add(new CsvColumn(column, CsvColumnType.PlainString));
			}
			index++;
		}
		return new CsvLine(csvColumns, parsed);
	}
}