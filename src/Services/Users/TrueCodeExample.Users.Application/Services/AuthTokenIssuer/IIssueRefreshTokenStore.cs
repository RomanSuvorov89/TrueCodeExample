using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public interface IIssueRefreshTokenStore
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
