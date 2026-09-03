namespace FluyAdmin.Application.DTOs;

public record LoginResult(string Token, Guid PlatformUserId, string Email, string FullName, string Role);
