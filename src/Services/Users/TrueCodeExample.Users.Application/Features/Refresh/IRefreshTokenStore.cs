using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Refresh;

public interface IRefreshTokenStore
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);

    ValueTask UpdateAsync(RefreshToken token, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
