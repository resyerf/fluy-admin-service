using FluyAdmin.Api.Models.Requests;
using FluyAdmin.Application.Commands.PlatformIdentity.ActivatePlatformUser;
using FluyAdmin.Application.Commands.PlatformIdentity.CreatePlatformUser;
using FluyAdmin.Application.Commands.PlatformIdentity.DeactivatePlatformUser;
using FluyAdmin.Application.Commands.PlatformIdentity.UpdatePlatformUserRole;
using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Queries.PlatformIdentity.GetPlatformUsers;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluyAdmin.Api.Controllers;

/// <summary>Identidad del personal de FLUY (CLAUDE.md §5) — separada de User/tenant (CODE.md §9.3).</summary>
[ApiController]
[Route("api/v1/platform-users")]
[Authorize]
public class PlatformUsersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PlatformUserSummary>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPlatformUsersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreatePlatformUserResult>> Create(CreatePlatformUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(
                new CreatePlatformUserCommand(request.Email, request.FullName, request.Password, request.Role), cancellationToken);
            return Ok(result);
        }
        catch (EmailAlreadyRegisteredException ex)
        {
            return Conflict(new { detail = ex.Message });
        }
    }

    [HttpPost("{id:guid}/role")]
    public async Task<ActionResult<UpdatePlatformUserRoleResult>> UpdateRole(
        Guid id, UpdatePlatformUserRoleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new UpdatePlatformUserRoleCommand(id, request.NewRole), cancellationToken);
            return Ok(result);
        }
        catch (PlatformUserNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<SetPlatformUserStatusResult>> Activate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new ActivatePlatformUserCommand(id), cancellationToken);
            return Ok(result);
        }
        catch (PlatformUserNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult<SetPlatformUserStatusResult>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new DeactivatePlatformUserCommand(id), cancellationToken);
            return Ok(result);
        }
        catch (PlatformUserNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }
}
