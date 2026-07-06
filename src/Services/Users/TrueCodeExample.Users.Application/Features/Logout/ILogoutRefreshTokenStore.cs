using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Logout;

public interface ILogoutRefreshTokenStore
{
    Task<IReadOnlyList<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken);

    ValueTask UpdateAsync(RefreshToken token, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
