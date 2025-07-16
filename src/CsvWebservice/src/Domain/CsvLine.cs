namespace Bison.CsvWebservice.Domain
{
	public class CsvLine
	{
		private List<CsvColumn> _columns;
		
		public IReadOnlyCollection<CsvColumn> Columns { get => _columns.AsReadOnly(); }
		
		public char Delimiter { get; private set; } = ',';

		public CsvLineStatus Status { get; private set; } = CsvLineStatus.NotFound;

		public CsvLine(List<CsvColumn> columns)
		{
			_columns = columns;
		}

		public CsvLine(List<CsvColumn> columns, CsvLineStatus status)
		{
			_columns = columns;
			Status = status;
		}

		public void SetStatus(CsvLineStatus status) =>
			Status = status;

		public override string ToString()
		{
			return string.Join(Delimiter, Columns.Select(c => c.Value.ToString()));
		}
	}
}
