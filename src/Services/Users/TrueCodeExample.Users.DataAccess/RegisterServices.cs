using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Application.Features.Logout;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Application.Features.Register;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.DataAccess.Repositories;

namespace TrueCodeExample.Users.DataAccess;

public static class RegisterServices
{
    private const string ConnectionStringName = "UsersDb";

    public static IServiceCollection AddUsersDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is missing.");

        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<UserRepository>();
        services.AddScoped<IRegisterUserStore>(sp => sp.GetRequiredService<UserRepository>());
        services.AddScoped<ILoginUserStore>(sp => sp.GetRequiredService<UserRepository>());
        services.AddScoped<IRefreshUserStore>(sp => sp.GetRequiredService<UserRepository>());

        services.AddScoped<RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenStore>(sp => sp.GetRequiredService<RefreshTokenRepository>());
        services.AddScoped<ILogoutRefreshTokenStore>(sp => sp.GetRequiredService<RefreshTokenRepository>());
        services.AddScoped<IIssueRefreshTokenStore>(sp => sp.GetRequiredService<RefreshTokenRepository>());

        return services;
    }
}
