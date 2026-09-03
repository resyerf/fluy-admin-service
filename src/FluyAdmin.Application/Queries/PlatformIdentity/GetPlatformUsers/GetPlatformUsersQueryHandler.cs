using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Queries.PlatformIdentity.GetPlatformUsers;

public class GetPlatformUsersQueryHandler(IPlatformUserRepository platformUsers)
    : IQueryHandler<GetPlatformUsersQuery, IReadOnlyCollection<PlatformUserSummary>>
{
    public Task<IReadOnlyCollection<PlatformUserSummary>> Handle(GetPlatformUsersQuery query, CancellationToken cancellationToken) =>
        platformUsers.GetAllAsync(cancellationToken);
}
