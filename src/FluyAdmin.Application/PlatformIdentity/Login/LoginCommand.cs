using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.PlatformIdentity.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
