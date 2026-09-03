using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface IPlanRepository
{
    Task<Plan?> GetActiveByCodeAsync(string code, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlanSummary>> GetAllWithFeaturesAsync(CancellationToken cancellationToken);
}
