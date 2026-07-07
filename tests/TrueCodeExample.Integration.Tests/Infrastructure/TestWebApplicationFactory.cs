using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace TrueCodeExample.Integration.Tests.Infrastructure;

internal static class TestWebApplicationFactory
{
    private const string JwtSecret = "integration-test-secret-key-32-chars-min";

    public static WebApplicationFactory<UsersApiProgram> CreateUsersFactory(string usersConnectionString)
        => new UsersApiFactory(usersConnectionString);

    public static WebApplicationFactory<FinanceApiProgram> CreateFinanceFactory(string financeConnectionString)
        => new FinanceApiFactory(financeConnectionString);

    private sealed class UsersApiFactory(string usersConnectionString) : WebApplicationFactory<UsersApiProgram>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            SetCommonEnvironment();
            Environment.SetEnvironmentVariable("ConnectionStrings__UsersDb", usersConnectionString);
            return base.CreateHost(builder);
        }
    }

    private sealed class FinanceApiFactory(string financeConnectionString) : WebApplicationFactory<FinanceApiProgram>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            SetCommonEnvironment();
            Environment.SetEnvironmentVariable("ConnectionStrings__FinanceDb", financeConnectionString);
            return base.CreateHost(builder);
        }
    }

    private static void SetCommonEnvironment()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Development);
        Environment.SetEnvironmentVariable("Jwt__Issuer", "TrueCode");
        Environment.SetEnvironmentVariable("Jwt__Audience", "TrueCodeClients");
        Environment.SetEnvironmentVariable("Jwt__AccessTokenMinutes", "15");
        Environment.SetEnvironmentVariable("Jwt__RefreshTokenDays", "7");
        Environment.SetEnvironmentVariable("Jwt__SecretKey", JwtSecret);
        Environment.SetEnvironmentVariable("Seq__ServerUrl", string.Empty);
    }
}
