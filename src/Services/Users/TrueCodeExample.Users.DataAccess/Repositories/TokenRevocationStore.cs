using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Repositories;

public sealed class TokenRevocationStore(UsersDbContext dbContext) : ITokenRevocationStore
{
    public async Task RevokeAsync(string jti, DateTime expiresAtUtc, CancellationToken cancellationToken)
    {
        if (await dbContext.RevokedTokens.AnyAsync(x => x.Jti == jti, cancellationToken))
        {
            return;
        }

        await dbContext.RevokedTokens.AddAsync(new RevokedToken(jti, expiresAtUtc), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken)
        => dbContext.RevokedTokens.AnyAsync(x => x.Jti == jti, cancellationToken);
}
