using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.PlatformIdentity.Login;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluyAdmin.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(ISender sender) : ControllerBase
{
    public record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResult>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
            return Ok(result);
        }
        catch (AuthenticationFailedException ex)
        {
            return Unauthorized(new { detail = ex.Message });
        }
    }
}
