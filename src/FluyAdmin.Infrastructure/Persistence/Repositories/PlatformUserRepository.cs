using FluyAdmin.Application.Common.Interfaces.Repositories;
using FluyAdmin.Domain.PlatformIdentity;
using Microsoft.EntityFrameworkCore;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class PlatformUserRepository(PlatformDbContext db) : IPlatformUserRepository
{
    public Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        db.PlatformUsers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
