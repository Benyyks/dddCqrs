namespace Bison.CsvWebservice.Web.Options
{
	public class RateLimitOptions
	{
		public bool Enabled { get; set; } = false;
		public int WindowInMinutes { get; set; } = 60;
		public int PermitLimit { get; set; } = 100;
	}
}
