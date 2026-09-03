using System.Security.Claims;
using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Enums;
using Fluy.SharedKernel.Dispatching;
using Fluy.SharedKernel.Security;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.PlatformIdentity.Login;

public class LoginCommandHandler(
    IPlatformUserRepository platformUsers,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var user = await platformUsers.GetByEmailAsync(email, cancellationToken);

        if (user is null || user.Status != PlatformUserStatus.Active || !passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new AuthenticationFailedException();
        }

        List<Claim> claims =
        [
            new("sub", user.Id.ToString()),
            new("email", user.Email),
            new("jti", Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        ];

        var token = jwtTokenGenerator.GenerateToken(claims);

        return new LoginResult(token, user.Id, user.Email, user.FullName, user.Role.ToString());
    }
}
