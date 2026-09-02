namespace FluyAdmin.Domain.Billing;

/// <summary>Estados de CLAUDE.md §11.</summary>
public enum SubscriptionStatus
{
    Trial = 0,
    Active = 1,
    PastDue = 2,
    Suspended = 3,
    Cancelled = 4,
    Expired = 5
}
