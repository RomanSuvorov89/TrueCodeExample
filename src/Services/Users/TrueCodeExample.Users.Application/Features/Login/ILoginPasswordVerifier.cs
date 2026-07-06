namespace TrueCodeExample.Users.Application.Features.Login;

public interface ILoginPasswordVerifier
{
    bool Verify(string password, string passwordHash);
}
