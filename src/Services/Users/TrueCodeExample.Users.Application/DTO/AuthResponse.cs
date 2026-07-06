namespace TrueCodeExample.Users.Application.DTO;

public sealed record AuthResponse(
    Guid UserId,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);
