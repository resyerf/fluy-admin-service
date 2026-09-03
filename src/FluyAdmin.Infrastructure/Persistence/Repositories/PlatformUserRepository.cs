using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FluyAdmin.Infrastructure.Persistence.Context;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class PlatformUserRepository(PlatformDbContext db) : IPlatformUserRepository
{
    public Task<PlatformUser?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        db.PlatformUsers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<PlatformUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.PlatformUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<bool> EmailTakenAsync(string email, CancellationToken cancellationToken) =>
        db.PlatformUsers.AnyAsync(u => u.Email == email, cancellationToken);

    public void Add(PlatformUser platformUser) => db.PlatformUsers.Add(platformUser);

    public async Task<IReadOnlyCollection<PlatformUserSummary>> GetAllAsync(CancellationToken cancellationToken) =>
        await db.PlatformUsers.AsNoTracking()
            .OrderBy(u => u.FullName)
            .Select(u => new PlatformUserSummary(u.Id, u.Email, u.FullName, u.Role.ToString(), u.Status.ToString()))
            .ToListAsync(cancellationToken);
}
