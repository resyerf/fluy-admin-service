namespace FluyAdmin.Application.Common.Exceptions;

public class SubdomainAlreadyTakenException(string subdomain)
    : Exception($"El subdominio '{subdomain}' ya está en uso por otro tenant.");
