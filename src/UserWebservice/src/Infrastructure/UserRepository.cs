using Bison.UserWebservice.Application.Abstractions;
using Bison.UserWebservice.Domain;

namespace Bison.UserWebservice.Infrastructure
{
	public class UserRepository : IUserRepository
	{
		public IAsyncEnumerable<User> GetAllAsync()
		{
			return DoNotTouchClass.GetUserDataTask();
		}

		public async Task<User> GetByIdAsync(Guid id)
		{
			await foreach (var user in GetAllAsync())
			{
				if (user.Id == id)
				{
					return user;
				}
			}

			// Return null or throw an exception if the user is not found
			return null;
		}
	}
}
