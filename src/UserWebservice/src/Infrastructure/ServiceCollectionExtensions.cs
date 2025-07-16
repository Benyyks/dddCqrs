using Bison.UserWebservice.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bison.UserWebservice.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();

			return services;
		}
	}
}
