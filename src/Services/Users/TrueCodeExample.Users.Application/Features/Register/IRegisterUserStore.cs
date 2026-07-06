using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Register;

public interface IRegisterUserStore
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
