using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.DataAccess.Repositories;

namespace TrueCodeExample.Users.DataAccess;

public static class DependencyInjection
{
    private const string ConnectionStringName = "UsersDb";

    public static IServiceCollection AddUsersDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is missing.");

        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRevocationStore, TokenRevocationStore>();

        return services;
    }
}
