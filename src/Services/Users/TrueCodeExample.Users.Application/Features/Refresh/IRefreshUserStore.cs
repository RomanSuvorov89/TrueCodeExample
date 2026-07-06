using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Refresh;

public interface IRefreshUserStore
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
