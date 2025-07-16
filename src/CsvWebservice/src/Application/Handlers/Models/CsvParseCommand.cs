using MediatR;

namespace Bison.CsvWebservice.Application.Handlers.Models
{
	public class CsvParseCommand : IRequest<CsvParsedReport>
	{
		public Stream Content { get; set; }
		public long ContentLength { get; set; }
	}
}
