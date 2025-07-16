namespace Bison.CsvWebservice.Application.Handlers.Models
{
	public class CsvParsedReport
	{
		public int Total { get; set; }

		public int Found { get; set; }

		public int Errors { get; set; }

		public List<Dictionary<string,string>> ValueLines { get; set; }
	}
}
