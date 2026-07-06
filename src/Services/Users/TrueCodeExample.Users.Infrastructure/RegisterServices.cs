using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Application.Features.Register;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.Infrastructure.Security;

namespace TrueCodeExample.Users.Infrastructure;

public static class RegisterServices
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<PasswordHasher>();
        services.AddSingleton<IRegisterPasswordHasher>(sp => sp.GetRequiredService<PasswordHasher>());
        services.AddSingleton<ILoginPasswordVerifier>(sp => sp.GetRequiredService<PasswordHasher>());

        services.AddSingleton<JwtTokenService>();
        services.AddSingleton<IAccessTokenGenerator>(sp => sp.GetRequiredService<JwtTokenService>());
        services.AddSingleton<IRefreshTokenGenerator>(sp => sp.GetRequiredService<JwtTokenService>());
        services.AddSingleton<IRefreshTokenHasher>(sp => sp.GetRequiredService<JwtTokenService>());

        return services;
    }
}
