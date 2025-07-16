using Bison.UserWebservice.Application.Abstractions;
using Bison.UserWebservice.Application.Handlers.Models;
using Bison.UserWebservice.Domain;
using MediatR;

namespace Bison.UserWebservice.Application.Handlers.Queries
{
	internal class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
	{
		private readonly IUserRepository _userRepository;

		public GetUserByIdHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			return await _userRepository.GetByIdAsync(request.UserId);
		
		}
	}
}
