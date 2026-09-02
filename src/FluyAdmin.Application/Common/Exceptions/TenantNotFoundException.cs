namespace FluyAdmin.Application.Common.Exceptions;

public class TenantNotFoundException(Guid tenantId)
    : Exception($"No existe el tenant '{tenantId}'.");
