using Bison.CsvWebservice.Domain;

namespace Bison.CsvWebservice.Application.Abstractions;

public interface IColumnInterpretor
{
    Task<CsvColumn> GetValueAsync(string value);
}