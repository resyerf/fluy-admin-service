namespace FluyAdmin.Application.DTOs;

public record PlatformUserSummary(Guid Id, string Email, string FullName, string Role, string Status);
