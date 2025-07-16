namespace Bison.CsvWebservice.Application.Options
{
	public class CachingOptions
	{
		public bool UseCachePreload { get; set; }
		public int CacheDurationInMinutes { get; set; } = 360; // Default to 6 hours
	}
}
