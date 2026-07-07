using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TrueCodeExample.Integration.Tests.Infrastructure;

namespace TrueCodeExample.Integration.Tests.Users;

[Collection("Integration")]
public sealed class UsersApiTests(PostgresFixture postgres)
{
    [Fact]
    public async Task Register_Login_And_Refresh_Workflow()
    {
        await using var factory = TestWebApplicationFactory.CreateUsersFactory(postgres.UsersConnectionString);
        using var client = factory.CreateClient();

        var name = $"user-{Guid.NewGuid():N}"[..20];
        var registerResponse = await client.PostAsJsonAsync("/auth/register", new { name, password = "secret12" });
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        auth.Should().NotBeNull();
        auth!.AccessToken.Should().NotBeNullOrWhiteSpace();
        auth.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var loginResponse = await client.PostAsJsonAsync("/auth/login", new { name, password = "secret12" });
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var refreshResponse = await client.PostAsJsonAsync("/auth/refresh", new { refreshToken = auth.RefreshToken });
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var refreshed = await refreshResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        refreshed!.RefreshToken.Should().NotBe(auth.RefreshToken);

        var reuseOldRefresh = await client.PostAsJsonAsync("/auth/refresh", new { refreshToken = auth.RefreshToken });
        reuseOldRefresh.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Register_DuplicateUser_ReturnsConflict()
    {
        await using var factory = TestWebApplicationFactory.CreateUsersFactory(postgres.UsersConnectionString);
        using var client = factory.CreateClient();

        var name = $"dup-{Guid.NewGuid():N}"[..20];
        var first = await client.PostAsJsonAsync("/auth/register", new { name, password = "secret12" });
        first.StatusCode.Should().Be(HttpStatusCode.OK);

        var second = await client.PostAsJsonAsync("/auth/register", new { name, password = "secret12" });
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        await using var factory = TestWebApplicationFactory.CreateUsersFactory(postgres.UsersConnectionString);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private sealed record AuthResponseDto(
        Guid UserId,
        string AccessToken,
        DateTime AccessTokenExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc);
}
