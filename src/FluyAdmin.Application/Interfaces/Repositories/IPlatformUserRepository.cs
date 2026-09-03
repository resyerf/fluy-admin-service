using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface IPlatformUserRepository
{
    Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<PlatformUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> EmailTakenAsync(string email, CancellationToken cancellationToken);
    void Add(PlatformUser platformUser);
    Task<IReadOnlyCollection<PlatformUserSummary>> GetAllAsync(CancellationToken cancellationToken);
}
