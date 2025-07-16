using Bison.CsvWebservice.Domain;

namespace Bison.CsvWebservice.Application.Abstractions;

public interface ICsvParser
{
	Task<CsvParsedFile> ParseCsv(Stream csvContent, int batchSize = 100);
}
