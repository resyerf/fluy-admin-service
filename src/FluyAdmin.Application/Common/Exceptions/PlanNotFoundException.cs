namespace FluyAdmin.Application.Common.Exceptions;

public class PlanNotFoundException(string planCode)
    : Exception($"El plan '{planCode}' no existe o no está activo.");
