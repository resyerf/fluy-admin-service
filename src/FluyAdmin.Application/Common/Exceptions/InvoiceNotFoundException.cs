namespace FluyAdmin.Application.Common.Exceptions;

public class InvoiceNotFoundException(Guid invoiceId)
    : Exception($"La factura '{invoiceId}' no existe.");
