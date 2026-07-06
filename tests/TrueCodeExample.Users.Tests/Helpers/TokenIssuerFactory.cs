using Moq;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Tests.Helpers;

internal static class TokenIssuerFactory
{
    public static TokenIssuer Create(
        out Mock<IAccessTokenGenerator> accessTokenGenerator,
        out Mock<IRefreshTokenGenerator> refreshTokenGenerator,
        out Mock<IIssueRefreshTokenStore> refreshTokenStore,
        Guid? userId = null)
    {
        accessTokenGenerator = new Mock<IAccessTokenGenerator>();
        refreshTokenGenerator = new Mock<IRefreshTokenGenerator>();
        refreshTokenStore = new Mock<IIssueRefreshTokenStore>();

        var id = userId ?? Guid.NewGuid();
        var accessExpiresAt = DateTime.UtcNow.AddMinutes(15);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(7);

        accessTokenGenerator
            .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns(new GeneratedToken("access-token", "jti", accessExpiresAt));

        refreshTokenGenerator
            .Setup(x => x.GenerateRefreshToken())
            .Returns(new GeneratedRefreshToken("refresh-token", "refresh-hash", refreshExpiresAt));

        refreshTokenStore
            .Setup(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        refreshTokenStore
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new TokenIssuer(
            accessTokenGenerator.Object,
            refreshTokenGenerator.Object,
            refreshTokenStore.Object);
    }
}
