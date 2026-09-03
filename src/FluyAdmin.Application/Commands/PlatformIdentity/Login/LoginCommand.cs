using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;
namespace FluyAdmin.Application.Commands.PlatformIdentity.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
