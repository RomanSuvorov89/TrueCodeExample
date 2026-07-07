using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Users.DataAccess;

namespace TrueCodeExample.Integration.Tests.Infrastructure;

public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string UsersConnectionString { get; private set; } = string.Empty;
    public string FinanceConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var baseConnectionString = _container.GetConnectionString();
        await using (var connection = new NpgsqlConnection(baseConnectionString))
        {
            await connection.OpenAsync();
            await using var usersDb = new NpgsqlCommand("CREATE DATABASE truecode_users", connection);
            await usersDb.ExecuteNonQueryAsync();
            await using var financeDb = new NpgsqlCommand("CREATE DATABASE truecode_finance", connection);
            await financeDb.ExecuteNonQueryAsync();
        }

        UsersConnectionString = BuildConnectionString(baseConnectionString!, "truecode_users");
        FinanceConnectionString = BuildConnectionString(baseConnectionString!, "truecode_finance");

        await MigrateAsync<UsersDbContext>(UsersConnectionString);
        await MigrateAsync<FinanceDbContext>(FinanceConnectionString);
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private static string BuildConnectionString(string baseConnectionString, string database)
    {
        var builder = new NpgsqlConnectionStringBuilder(baseConnectionString) { Database = database };
        return builder.ConnectionString;
    }

    private static async Task MigrateAsync<TContext>(string connectionString)
        where TContext : DbContext
    {
        var options = new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = (TContext)Activator.CreateInstance(typeof(TContext), options)!;
        await context.Database.MigrateAsync();
    }
}

[CollectionDefinition("Integration")]
public sealed class IntegrationCollection : ICollectionFixture<PostgresFixture>;
