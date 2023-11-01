using Application.Features.UserRole.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("user/addrole")]

        public async Task<ActionResult<Unit>> AddUserRoleToUser(string username,string rolename = "User") // Default role (User)
        {
            return Ok(await _mediator.Send(new CreateUserRoleCommand(username,rolename)));
        } 
    }
}
