using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Infrastructure.Security;

namespace TrueCodeExample.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }
}
