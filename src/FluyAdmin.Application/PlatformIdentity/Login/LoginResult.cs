namespace FluyAdmin.Application.PlatformIdentity.Login;

public record LoginResult(string Token, Guid PlatformUserId, string Email, string FullName, string Role);
