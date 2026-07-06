using System.Security.Claims;
using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Common.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirstValue("sub");

        return Guid.TryParse(value, out var userId) ? userId : throw new UnauthorizedDomainException();
    }

    public static string? GetJti(this ClaimsPrincipal principal)
        => principal.FindFirstValue("jti");

    public static DateTime? GetExpiration(this ClaimsPrincipal principal)
        => long.TryParse(principal.FindFirstValue("exp"), out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime
            : null;
}
