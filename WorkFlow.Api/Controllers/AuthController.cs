using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WorkFlow.Application.Commands.Auth;

namespace WorkFlow.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(LoginCommandHandler handler) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(result);
        }
    }
}
