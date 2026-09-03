using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.PlatformIdentity.GetPlatformUsers;

public sealed record GetPlatformUsersQuery : IQuery<IReadOnlyCollection<PlatformUserSummary>>;
