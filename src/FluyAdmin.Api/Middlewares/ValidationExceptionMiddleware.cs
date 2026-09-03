using FluentValidation;

namespace FluyAdmin.Api.Middlewares;

/// <summary>
/// Traduce el ValidationException que lanza Dispatcher (Fluy.SharedKernel) a un 400 con formato
/// ProblemDetails, igual que en fluy-service (CODE.md §4.23).
/// </summary>
public class ValidationExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                status = 400,
                title = "Uno o más campos no son válidos.",
                errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });
        }
    }
}
