using Fluy.SharedKernel;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Catálogo global de features de FLUY (CLAUDE.md §10) — ej. "workflow.advanced", "max.users".
/// No tenant-scoped: es el mismo catálogo para todos los tenants, igual que Permission en
/// fluy-service — lo que varía por tenant es si lo tiene contratado (PlanFeature vía su Subscription).
/// </summary>
public class Feature : BaseEntity, IAuditableEntity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public FeatureType Type { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Feature()
    {
    }

    public static Feature Create(string code, string name, FeatureType type)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("El código del feature es obligatorio.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del feature es obligatorio.", nameof(name));
        }

        return new Feature
        {
            Code = code.Trim().ToLowerInvariant(),
            Name = name.Trim(),
            Type = type
        };
    }
}
