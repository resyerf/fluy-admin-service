namespace FluyAdmin.Domain.Enums;

/// <summary>
/// A diferencia de Role en fluy-service (configurable por tenant), el equipo interno de FLUY es
/// pequeño y sus roles no varían por cliente — un enum simple es suficiente (CODE.md §9.3).
/// </summary>
public enum PlatformRole
{
    SuperAdmin = 0,
    BillingOps = 1,
    Support = 2,
    ReadOnly = 3
}
