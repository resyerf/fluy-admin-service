using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    void Add(Subscription subscription);
    void AddItem(SubscriptionItem item);
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<SubscriptionSummary>> GetAllAsync(CancellationToken cancellationToken);
}
