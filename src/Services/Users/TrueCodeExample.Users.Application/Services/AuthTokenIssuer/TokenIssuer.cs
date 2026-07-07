using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public sealed class TokenIssuer(
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IIssueRefreshTokenStore refreshTokens)
{
    public IssuedTokenPair CreateTokenPair(User user)
    {
        var accessToken = accessTokenGenerator.GenerateAccessToken(user);
        var refreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var refreshEntity = new RefreshToken(user.Id, refreshToken.TokenHash, refreshToken.ExpiresAtUtc);

        var response = new AuthResponse(
            user.Id,
            accessToken.AccessToken,
            accessToken.ExpiresAtUtc,
            refreshToken.Token,
            refreshToken.ExpiresAtUtc);

        return new IssuedTokenPair(response, refreshEntity);
    }

    public async ValueTask<AuthResponse> IssueAsync(User user, CancellationToken cancellationToken)
    {
        var pair = CreateTokenPair(user);
        await refreshTokens.AddAsync(pair.RefreshToken, cancellationToken);
        await refreshTokens.SaveChangesAsync(cancellationToken);
        return pair.Response;
    }
}
