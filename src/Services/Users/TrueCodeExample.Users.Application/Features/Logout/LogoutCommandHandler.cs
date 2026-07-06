using Mediator;
using TrueCodeExample.Users.Application.Abstractions;

namespace TrueCodeExample.Users.Application.Features.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ITokenRevocationStore _revocationStore;

    public LogoutCommandHandler(ITokenRevocationStore revocationStore)
    {
        _revocationStore = revocationStore;
    }

    public async ValueTask<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _revocationStore.RevokeAsync(request.Jti, request.ExpiresAtUtc, cancellationToken);
        return Unit.Value;
    }
}
