using FluentAssertions;
using Moq;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Domain.Entities;
using TrueCodeExample.Users.Domain.Exceptions;
using TrueCodeExample.Users.Tests.Helpers;

namespace TrueCodeExample.Users.Tests.Features.Refresh;

public sealed class RefreshTokenCommandHandlerTests
{
    private static readonly User ExistingUser = User.Restore(Guid.NewGuid(), "alice", "hashed-password");

    [Fact]
    public async Task Handle_WhenRefreshTokenIsValid_RotatesTokenAndReturnsNewTokens()
    {
        var users = new Mock<IRefreshUserStore>();
        var refreshTokens = new Mock<IRefreshTokenStore>();
        var tokenHasher = new Mock<IRefreshTokenHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _, ExistingUser.Id);

        var storedToken = RefreshToken.Restore(
            Guid.NewGuid(),
            ExistingUser.Id,
            "token-hash",
            DateTime.UtcNow.AddDays(7),
            DateTime.UtcNow.AddDays(-1),
            null);

        tokenHasher.Setup(x => x.HashRefreshToken("refresh-token")).Returns("token-hash");
        refreshTokens.Setup(x => x.GetByHashAsync("token-hash", It.IsAny<CancellationToken>())).ReturnsAsync(storedToken);
        users.Setup(x => x.GetByIdAsync(ExistingUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(ExistingUser);
        refreshTokens.Setup(x => x.TryRotateAsync(storedToken.Id, It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new RefreshTokenCommandHandler(
            users.Object,
            refreshTokens.Object,
            tokenHasher.Object,
            tokenIssuer);

        var response = await handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        response.UserId.Should().Be(ExistingUser.Id);
        response.AccessToken.Should().Be("access-token");
        response.RefreshToken.Should().Be("refresh-token");
        refreshTokens.Verify(x => x.TryRotateAsync(storedToken.Id, It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRotationFails_ThrowsInvalidRefreshTokenException()
    {
        var users = new Mock<IRefreshUserStore>();
        var refreshTokens = new Mock<IRefreshTokenStore>();
        var tokenHasher = new Mock<IRefreshTokenHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _, ExistingUser.Id);

        var storedToken = RefreshToken.Restore(
            Guid.NewGuid(),
            ExistingUser.Id,
            "token-hash",
            DateTime.UtcNow.AddDays(7),
            DateTime.UtcNow.AddDays(-1),
            null);

        tokenHasher.Setup(x => x.HashRefreshToken("refresh-token")).Returns("token-hash");
        refreshTokens.Setup(x => x.GetByHashAsync("token-hash", It.IsAny<CancellationToken>())).ReturnsAsync(storedToken);
        users.Setup(x => x.GetByIdAsync(ExistingUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(ExistingUser);
        refreshTokens.Setup(x => x.TryRotateAsync(storedToken.Id, It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new RefreshTokenCommandHandler(
            users.Object,
            refreshTokens.Object,
            tokenHasher.Object,
            tokenIssuer);

        var act = async () => await handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenNotFound_ThrowsInvalidRefreshTokenException()
    {
        var users = new Mock<IRefreshUserStore>();
        var refreshTokens = new Mock<IRefreshTokenStore>();
        var tokenHasher = new Mock<IRefreshTokenHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        tokenHasher.Setup(x => x.HashRefreshToken("refresh-token")).Returns("token-hash");
        refreshTokens.Setup(x => x.GetByHashAsync("token-hash", It.IsAny<CancellationToken>())).ReturnsAsync((RefreshToken?)null);

        var handler = new RefreshTokenCommandHandler(
            users.Object,
            refreshTokens.Object,
            tokenHasher.Object,
            tokenIssuer);

        var act = async () => await handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsRevoked_ThrowsInvalidRefreshTokenException()
    {
        var users = new Mock<IRefreshUserStore>();
        var refreshTokens = new Mock<IRefreshTokenStore>();
        var tokenHasher = new Mock<IRefreshTokenHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        var revokedToken = RefreshToken.Restore(
            Guid.NewGuid(),
            ExistingUser.Id,
            "token-hash",
            DateTime.UtcNow.AddDays(7),
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);

        tokenHasher.Setup(x => x.HashRefreshToken("refresh-token")).Returns("token-hash");
        refreshTokens.Setup(x => x.GetByHashAsync("token-hash", It.IsAny<CancellationToken>())).ReturnsAsync(revokedToken);

        var handler = new RefreshTokenCommandHandler(
            users.Object,
            refreshTokens.Object,
            tokenHasher.Object,
            tokenIssuer);

        var act = async () => await handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ThrowsInvalidRefreshTokenException()
    {
        var users = new Mock<IRefreshUserStore>();
        var refreshTokens = new Mock<IRefreshTokenStore>();
        var tokenHasher = new Mock<IRefreshTokenHasher>();
        var tokenIssuer = TokenIssuerFactory.Create(out _, out _, out _);

        var storedToken = RefreshToken.Restore(
            Guid.NewGuid(),
            ExistingUser.Id,
            "token-hash",
            DateTime.UtcNow.AddDays(7),
            DateTime.UtcNow.AddDays(-1),
            null);

        tokenHasher.Setup(x => x.HashRefreshToken("refresh-token")).Returns("token-hash");
        refreshTokens.Setup(x => x.GetByHashAsync("token-hash", It.IsAny<CancellationToken>())).ReturnsAsync(storedToken);
        users.Setup(x => x.GetByIdAsync(ExistingUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var handler = new RefreshTokenCommandHandler(
            users.Object,
            refreshTokens.Object,
            tokenHasher.Object,
            tokenIssuer);

        var act = async () => await handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidRefreshTokenException>();
    }
}
