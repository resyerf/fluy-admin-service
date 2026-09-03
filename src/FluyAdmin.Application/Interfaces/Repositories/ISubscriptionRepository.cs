using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    void Add(Subscription subscription);
    Task<IReadOnlyCollection<SubscriptionSummary>> GetAllAsync(CancellationToken cancellationToken);
}
