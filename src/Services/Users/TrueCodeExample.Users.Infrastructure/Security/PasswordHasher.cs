using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Application.Features.Register;

namespace TrueCodeExample.Users.Infrastructure.Security;

public sealed class PasswordHasher : IRegisterPasswordHasher, ILoginPasswordVerifier
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
