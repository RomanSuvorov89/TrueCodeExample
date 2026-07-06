using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Abstractions;

public interface ITokenService
{
    GeneratedToken GenerateToken(User user);
}
