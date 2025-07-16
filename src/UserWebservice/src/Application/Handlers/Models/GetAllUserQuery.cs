using Bison.UserWebservice.Domain;
using MediatR;

namespace Bison.UserWebservice.Application.Handlers.Models
{
	//we don't use User DTO for simplicity, but in a real application we would use a DTO to avoid exposing the domain model directly
	public class GetAllUserQuery : IStreamRequest<User>
	{
	}
}
