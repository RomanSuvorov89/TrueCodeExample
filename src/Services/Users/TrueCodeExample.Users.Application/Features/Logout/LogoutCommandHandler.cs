using Mediator;

namespace TrueCodeExample.Users.Application.Features.Logout;

public sealed class LogoutCommandHandler(ILogoutRefreshTokenStore refreshTokens) : IRequestHandler<LogoutCommand>
{
    public async ValueTask<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokens = await refreshTokens.GetActiveByUserAsync(request.UserId, cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke();
            await refreshTokens.UpdateAsync(token, cancellationToken);
        }

        await refreshTokens.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
