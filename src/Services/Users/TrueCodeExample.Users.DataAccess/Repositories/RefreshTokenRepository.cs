using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Users.Application.Features.Logout;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.DataAccess.Entities;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Repositories;

public sealed class RefreshTokenRepository(UsersDbContext dbContext) : 
    IRefreshTokenStore, 
    ILogoutRefreshTokenStore,
    IIssueRefreshTokenStore
{
    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken)
        => await dbContext.RefreshTokens.AddAsync(token.ToEntity(), cancellationToken);

    public ValueTask UpdateAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        dbContext.RefreshTokens.Update(token.ToEntity());
        return ValueTask.CompletedTask;
    }

    public async Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken)
    {
        var entity = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
        return entity?.ToDomain();
    }

    public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await dbContext.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        return entities.Select(x => x.ToDomain()).ToList();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
