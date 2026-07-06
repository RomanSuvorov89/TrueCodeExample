using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Features.Login;

public interface ILoginUserStore
{
    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
