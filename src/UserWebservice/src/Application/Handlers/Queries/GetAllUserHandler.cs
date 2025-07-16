using Bison.UserWebservice.Application.Abstractions;
using Bison.UserWebservice.Application.Handlers.Models;
using Bison.UserWebservice.Domain;
using MediatR;

namespace Bison.UserWebservice.Application.Handlers.Queries
{
	public class GetAllUserHandler : IStreamRequestHandler<GetAllUserQuery, User>
	{
		private readonly IUserRepository _userRepository;

		public GetAllUserHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IAsyncEnumerable<User> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
		{
			return _userRepository.GetAllAsync();
		}
	}
}
