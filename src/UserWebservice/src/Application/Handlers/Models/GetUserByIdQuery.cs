using Bison.UserWebservice.Domain;
using MediatR;

namespace Bison.UserWebservice.Application.Handlers.Models
{
	public class GetUserByIdQuery : IRequest<User>
	{
		public Guid UserId { get; set; }
		public GetUserByIdQuery(Guid userId)
		{
			UserId = userId;
		}
	}
}
