namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public interface IRefreshTokenGenerator
{
    GeneratedRefreshToken GenerateRefreshToken();
}
