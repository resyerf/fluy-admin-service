using Fluy.SharedKernel;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Identidad del personal de FLUY. Deliberadamente separada de User (fluy-service): un
/// PlatformUser no pertenece a ningún tenant y nunca debe autenticarse con el mismo JWT que un
/// usuario de tenant (CODE.md §9.3).
/// </summary>
public class PlatformUser : AggregateRoot, IAuditableEntity
{
    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public PlatformRole Role { get; private set; }
    public PlatformUserStatus Status { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private PlatformUser()
    {
    }

    public static PlatformUser Create(string email, string fullName, string passwordHash, PlatformRole role)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("El email es obligatorio.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(fullName));
        }

        return new PlatformUser
        {
            Email = email.Trim().ToLowerInvariant(),
            FullName = fullName.Trim(),
            PasswordHash = passwordHash,
            Role = role,
            Status = PlatformUserStatus.Active
        };
    }
}
