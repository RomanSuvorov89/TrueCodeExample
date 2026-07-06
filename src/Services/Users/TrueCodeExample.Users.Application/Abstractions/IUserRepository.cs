using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
