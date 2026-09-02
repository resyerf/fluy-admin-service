using FluyAdmin.Domain.PlatformIdentity;

namespace FluyAdmin.Application.Common.Interfaces.Repositories;

public interface IPlatformUserRepository
{
    Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
