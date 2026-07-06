namespace TrueCodeExample.Users.Application.Contracts;

public sealed record AuthResponse(Guid UserId, string AccessToken, DateTime ExpiresAtUtc);
