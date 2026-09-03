namespace FluyAdmin.Application.Common.Exceptions;

public class SubscriptionNotFoundException(Guid subscriptionId)
    : Exception($"La suscripción '{subscriptionId}' no existe.");
