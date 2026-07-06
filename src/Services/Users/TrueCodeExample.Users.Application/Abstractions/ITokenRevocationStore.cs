namespace TrueCodeExample.Users.Application.Abstractions;

public interface ITokenRevocationStore
{
    Task RevokeAsync(string jti, DateTime expiresAtUtc, CancellationToken cancellationToken);

    Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken);
}
