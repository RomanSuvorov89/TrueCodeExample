namespace TrueCodeExample.Users.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }

    public RefreshToken(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private RefreshToken(Guid id, Guid userId, string tokenHash, DateTime expiresAtUtc, DateTime createdAtUtc, DateTime? revokedAtUtc)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = createdAtUtc;
        RevokedAtUtc = revokedAtUtc;
    }

    public static RefreshToken Restore(Guid id, Guid userId, string tokenHash, DateTime expiresAtUtc, DateTime createdAtUtc, DateTime? revokedAtUtc)
        => new(id, userId, tokenHash, expiresAtUtc, createdAtUtc, revokedAtUtc);

    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;

    public void Revoke() => RevokedAtUtc = DateTime.UtcNow;
}
