namespace Bison.CsvWebservice.Domain
{
	public class CsvParsedFile
	{
		private List<CsvLine> _lines;

		public IReadOnlyCollection<CsvLine> Lines { get => _lines.AsReadOnly(); }

		public CsvHeader Header { get; private set; } = new CsvHeader();

		public int TotalLines => Lines?.Count ?? 0;

		public CsvParsedFile()
		{
			_lines = new List<CsvLine>();
		}

		public void SetHeader(string header, char delimiter)
		{
			if (string.IsNullOrWhiteSpace(header))
				throw new ArgumentNullException(nameof(header), "Header cannot be null.");
			Header = new CsvHeader(header, delimiter);
		}

		public void AddLines(IEnumerable<CsvLine> lines)
		{
			if (lines == null)
				throw new ArgumentNullException(nameof(lines), "Lines cannot be null.");
			_lines.AddRange(lines);
		}

		public Dictionary<string, string> RenderLine(int index)
		{
			if (index < 0 || index >= TotalLines)
				throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

			var line = _lines[index];
			var renderedLine = new Dictionary<string, string>();
			for (int i = 0; i < Header.Headers.Count; i++)
			{
				if (i < line.Columns.Count)
				{
					renderedLine[Header.Headers[i]] = line.Columns.ElementAt(i).Value;
				}
				else
				{
					renderedLine[Header.Headers[i]] = string.Empty; // Handle missing columns
				}
			}
			return renderedLine;
		}
	}
}
