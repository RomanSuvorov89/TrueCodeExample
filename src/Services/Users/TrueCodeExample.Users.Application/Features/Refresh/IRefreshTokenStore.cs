using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Refresh;

public interface IRefreshTokenStore
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);

    Task<bool> TryRotateAsync(Guid existingTokenId, RefreshToken newToken, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
