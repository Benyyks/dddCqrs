using Bison.UserWebservice.Domain;

namespace Bison.UserWebservice.Application.Abstractions
{
	public interface IUserRepository
	{
		IAsyncEnumerable<User> GetAllAsync();

		Task<User> GetByIdAsync(Guid id);
	}
}
