using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bison.UserWebservice.Application
{

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			return services;
		}
	}
}
