using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Common.Behaviors;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

namespace TrueCodeExample.Users.Application;

public static class RegisterServices
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        var assembly = typeof(RegisterServices).Assembly;

        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<TokenIssuer>();

        return services;
    }
}
