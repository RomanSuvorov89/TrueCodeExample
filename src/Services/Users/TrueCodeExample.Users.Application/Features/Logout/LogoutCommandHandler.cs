using Mediator;
using TrueCodeExample.Users.Application.Abstractions;

namespace TrueCodeExample.Users.Application.Features.Logout;

public sealed class LogoutCommandHandler(ITokenRevocationStore revocationStore) : IRequestHandler<LogoutCommand>
{
    public async ValueTask<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await revocationStore.RevokeAsync(request.Jti, request.ExpiresAtUtc, cancellationToken);
        return Unit.Value;
    }
}
