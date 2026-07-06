using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public interface IAccessTokenGenerator
{
    GeneratedToken GenerateAccessToken(User user);
}
