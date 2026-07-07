using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Common.Behaviors;

namespace TrueCodeExample.Finance.Application;

public static class RegisterServices
{
    public static IServiceCollection AddFinanceApplication(this IServiceCollection services)
    {
        var assembly = typeof(RegisterServices).Assembly;

        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
