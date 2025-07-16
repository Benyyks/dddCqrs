namespace Bison.CsvWebservice.Domain
{
	public class CsvColumn
	{
		public string Value { get; private set; }

		public CsvColumnType Type { get; private set; }

		public bool Parsed => Type != CsvColumnType.PlainString;

		public CsvColumn(string value, CsvColumnType type)
		{
			Value = value;
			Type = type;
		}
	}
}
