using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Entities;

internal static class UserEntityMappings
{
    public static User ToDomain(this UserEntity entity)
        => User.Restore(entity.Id, entity.Name, entity.PasswordHash);

    public static UserEntity ToEntity(this User user)
        => new()
        {
            Id = user.Id,
            Name = user.Name,
            PasswordHash = user.PasswordHash
        };

    public static RefreshToken ToDomain(this RefreshTokenEntity entity)
        => RefreshToken.Restore(entity.Id, entity.UserId, entity.TokenHash, entity.ExpiresAtUtc, entity.CreatedAtUtc, entity.RevokedAtUtc);

    public static RefreshTokenEntity ToEntity(this RefreshToken token)
        => new()
        {
            Id = token.Id,
            UserId = token.UserId,
            TokenHash = token.TokenHash,
            ExpiresAtUtc = token.ExpiresAtUtc,
            CreatedAtUtc = token.CreatedAtUtc,
            RevokedAtUtc = token.RevokedAtUtc
        };
}
