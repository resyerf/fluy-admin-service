namespace FluyAdmin.Api.Models.Requests;

public record CreatePlatformUserRequest(string Email, string FullName, string Password, string Role);
