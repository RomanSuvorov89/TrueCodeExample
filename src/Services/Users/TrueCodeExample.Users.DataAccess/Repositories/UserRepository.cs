using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;

    public UserRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => _dbContext.Users.AnyAsync(x => x.Name == name, cancellationToken);

    public Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _dbContext.Users.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
        => await _dbContext.Users.AddAsync(user, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
