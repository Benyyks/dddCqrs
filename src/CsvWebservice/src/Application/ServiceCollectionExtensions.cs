using Bison.CsvWebservice.Application.Abstractions;
using Bison.CsvWebservice.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bison.CsvWebservice.Application
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, Uri userWebServiceUrl)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			services.AddScoped<ICsvParser, CsvParser>();
			services.AddHttpClient<IColumnInterpretor, ColumnInterpretor>(client =>
			{
				client.BaseAddress = userWebServiceUrl;
			});

			return services;
		}
	}
}
