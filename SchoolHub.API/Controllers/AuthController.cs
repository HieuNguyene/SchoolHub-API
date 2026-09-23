using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Application.Features.Auth.Commands.Login;
using SchoolHub.Application.Features.Auth.Commands.RefreshToken;
using SchoolHub.Application.Features.Auth.Commands.Register;
using SchoolHub.Application.Features.Auth.Commands.RevokeToken;

namespace SchoolHub.API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IMediator mediator) : ApiControllerBase
    {
        // 1. API Đăng ký tài khoản
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResult(result);
        }

        // 2. API Đăng nhập (Nhận AccessToken + RefreshToken)
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResult(result);
        }

        // 3. API Làm mới Token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResult(result);
        }
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResult(result);
        }
    }
}