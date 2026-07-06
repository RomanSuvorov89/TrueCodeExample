namespace TrueCodeExample.Users.Application.Features.Register;

public interface IRegisterPasswordHasher
{
    string Hash(string password);
}
