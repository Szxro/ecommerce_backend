using Application.Features.Users.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("user/newuser")]

        public async Task<ActionResult<Unit>> CreateNewUser(CreateUserCommand newUser)
        {
            return Ok(await _mediator.Send(newUser));
        }
    }
}
