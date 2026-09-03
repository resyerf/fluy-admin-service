namespace FluyAdmin.Application.Common.Exceptions;

public class PlatformUserNotFoundException(Guid platformUserId)
    : Exception($"El PlatformUser '{platformUserId}' no existe.");
