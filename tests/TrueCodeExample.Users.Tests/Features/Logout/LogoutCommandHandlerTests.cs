using FluentAssertions;
using Moq;
using TrueCodeExample.Users.Application.Features.Logout;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Tests.Features.Logout;

public sealed class LogoutCommandHandlerTests
{
    [Fact]
    public async Task Handle_RevokesAllActiveRefreshTokensForUser()
    {
        var refreshTokens = new Mock<ILogoutRefreshTokenStore>();
        var userId = Guid.NewGuid();

        var token1 = RefreshToken.Restore(Guid.NewGuid(), userId, "hash-1", DateTime.UtcNow.AddDays(7), DateTime.UtcNow, null);
        var token2 = RefreshToken.Restore(Guid.NewGuid(), userId, "hash-2", DateTime.UtcNow.AddDays(7), DateTime.UtcNow, null);

        refreshTokens
            .Setup(x => x.GetActiveByUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RefreshToken> { token1, token2 });

        refreshTokens
            .Setup(x => x.UpdateAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        refreshTokens
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new LogoutCommandHandler(refreshTokens.Object);

        await handler.Handle(new LogoutCommand(userId), CancellationToken.None);

        token1.RevokedAtUtc.Should().NotBeNull();
        token2.RevokedAtUtc.Should().NotBeNull();
        refreshTokens.Verify(x => x.UpdateAsync(token1, It.IsAny<CancellationToken>()), Times.Once);
        refreshTokens.Verify(x => x.UpdateAsync(token2, It.IsAny<CancellationToken>()), Times.Once);
        refreshTokens.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoActiveTokens_CompletesSuccessfully()
    {
        var refreshTokens = new Mock<ILogoutRefreshTokenStore>();
        var userId = Guid.NewGuid();

        refreshTokens
            .Setup(x => x.GetActiveByUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        refreshTokens
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new LogoutCommandHandler(refreshTokens.Object);

        var act = async () => await handler.Handle(new LogoutCommand(userId), CancellationToken.None);

        await act.Should().NotThrowAsync();
        refreshTokens.Verify(x => x.UpdateAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
        refreshTokens.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
