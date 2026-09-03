using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FluyAdmin.Infrastructure.Persistence.Context;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class PlatformUserRepository(PlatformDbContext db) : IPlatformUserRepository
{
    public Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        db.PlatformUsers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
