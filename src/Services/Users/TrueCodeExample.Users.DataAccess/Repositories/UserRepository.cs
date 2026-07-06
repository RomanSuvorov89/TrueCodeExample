using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Application.Features.Register;
using TrueCodeExample.Users.DataAccess.Entities;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Repositories;

public sealed class UserRepository(UsersDbContext dbContext)
    : IRegisterUserStore, ILoginUserStore, IRefreshUserStore
{
    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => dbContext.Users.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Users.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
        return entity?.ToDomain();
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity?.ToDomain();
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
        => await dbContext.Users.AddAsync(user.ToEntity(), cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
