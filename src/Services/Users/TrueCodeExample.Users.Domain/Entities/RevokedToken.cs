namespace TrueCodeExample.Users.Domain.Entities;

public class RevokedToken
{
    public string Jti { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }

    private RevokedToken()
    {
    }

    public RevokedToken(string jti, DateTime expiresAtUtc)
    {
        Jti = jti;
        ExpiresAtUtc = expiresAtUtc;
    }
}
