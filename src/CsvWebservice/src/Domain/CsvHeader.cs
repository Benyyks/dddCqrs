namespace Bison.CsvWebservice.Domain
{
	public class CsvHeader
	{

		public List<string> Headers { get; private set; }

		public char Delimiter { get; private set; } = ',';

		public CsvHeader()
		{
			//default headers
			Headers = new List<string>()
			{
				"User Id",
				"Prix"
			};
		}

		public CsvHeader(List<string> headers)
		{
			Headers = headers;
		}

		public CsvHeader(string headersLine, char delimiter)
		{
			Delimiter = delimiter;
			Headers = headersLine.Split(delimiter).Select(h => h.Trim()).ToList();
		}

		public bool CheckHeader(string line) =>
			line == ToString();


		public override string ToString()
		{
			return string.Join(Delimiter, Headers);
		}
	}
}
