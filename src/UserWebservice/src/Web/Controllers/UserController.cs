using Bison.UserWebservice.Application.Handlers.Models;
using Bison.UserWebservice.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserWebservice.Web.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
	private readonly ISender _sender;
	public UserController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public IAsyncEnumerable<User> Get()
	{
		return _sender.CreateStream(new GetAllUserQuery());
	}

	[HttpGet("{id}")]
	[ProducesResponseType<User>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id)
	{
		User user = await _sender.Send(new GetUserByIdQuery(id));
		if(user is null)
		{
			return NotFound();
		}
		else
		{
			return Ok(user);
		}
	}
}
