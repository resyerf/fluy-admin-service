namespace FluyAdmin.Application.Common.Exceptions;

public class EmailAlreadyRegisteredException(string email)
    : Exception($"Ya existe un PlatformUser con el email '{email}'.");
