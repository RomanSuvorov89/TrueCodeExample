using Mediator;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Exceptions;

namespace TrueCodeExample.Users.Application.Features.Refresh;

public sealed class RefreshTokenCommandHandler(
    IRefreshUserStore users,
    IRefreshTokenStore refreshTokens,
    IRefreshTokenHasher tokenHasher,
    TokenIssuer tokenIssuer)
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async ValueTask<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = tokenHasher.HashRefreshToken(request.RefreshToken);

        var stored = await refreshTokens.GetByHashAsync(tokenHash, cancellationToken);
        if (stored is null || !stored.IsActive)
        {
            throw new InvalidRefreshTokenException();
        }

        var user = await users.GetByIdAsync(stored.UserId, cancellationToken)
                   ?? throw new InvalidRefreshTokenException();

        var pair = tokenIssuer.CreateTokenPair(user);
        var rotated = await refreshTokens.TryRotateAsync(stored.Id, pair.RefreshToken, cancellationToken);
        if (!rotated) throw new InvalidRefreshTokenException();

        return pair.Response;
    }
}
