namespace FluyAdmin.Domain.Billing;

/// <summary>
/// Boolean: el feature está contratado o no ("true"/"false" en PlanFeature.Value).
/// Limit: un número (o "unlimited") — ej. max.users, max.requests.month (CLAUDE.md §10).
/// </summary>
public enum FeatureType
{
    Boolean = 0,
    Limit = 1
}
