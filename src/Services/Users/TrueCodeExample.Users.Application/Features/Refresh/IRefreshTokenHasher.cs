namespace TrueCodeExample.Users.Application.Features.Refresh;

public interface IRefreshTokenHasher
{
    string HashRefreshToken(string refreshToken);
}
