using Bison.CsvWebservice.Domain;

namespace Bison.CsvWebservice.Application.Abstractions
{
	public interface IConcurrentCache
	{
		Task<CsvColumn> GetOrAddValueAsync(string key, Func<Task<CsvColumn>> fetchData);
	}
}
