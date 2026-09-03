namespace FluyAdmin.Application.DTOs;

public record CreatePlatformUserResult(Guid PlatformUserId, string Email, string Role);
