using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public sealed class TokenIssuer(
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IIssueRefreshTokenStore refreshTokens)
{
    public async ValueTask<AuthResponse> IssueAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = accessTokenGenerator.GenerateAccessToken(user);
        var refreshToken = refreshTokenGenerator.GenerateRefreshToken();

        await refreshTokens.AddAsync(new RefreshToken(user.Id, refreshToken.TokenHash, refreshToken.ExpiresAtUtc), cancellationToken);
        await refreshTokens.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            user.Id,
            accessToken.AccessToken,
            accessToken.ExpiresAtUtc,
            refreshToken.Token,
            refreshToken.ExpiresAtUtc);
    }
}
