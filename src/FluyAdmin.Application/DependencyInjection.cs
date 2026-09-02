using System.Reflection;
using Fluy.SharedKernel.Dispatching;
using Microsoft.Extensions.DependencyInjection;

namespace FluyAdmin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDispatcher(Assembly.GetExecutingAssembly());

        return services;
    }
}
