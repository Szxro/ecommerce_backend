using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Register;
using Domain.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("user/register")]

        public async Task<ActionResult<string>> CreateNewUser(RegisterUserCommand newUser)
        {
            return Ok(await _mediator.Send(newUser));
        }

        [HttpPost("user/login")]
        public async Task<ActionResult<TokenResponse>> LoginUser(string username,
                                                                 string password,
                                                                 CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new LoginUserCommand(username, password),cancellationToken));
        }
    }
}
