using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface IPlatformUserRepository
{
    Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
