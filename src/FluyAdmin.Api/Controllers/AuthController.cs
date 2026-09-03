using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.Commands.PlatformIdentity.Login;
using FluyAdmin.Application.DTOs;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluyAdmin.Api.Models.Requests;

namespace FluyAdmin.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(ISender sender) : ControllerBase
{

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
