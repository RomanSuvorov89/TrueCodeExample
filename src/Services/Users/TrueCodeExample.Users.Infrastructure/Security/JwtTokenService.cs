using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Infrastructure.Security;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _options = options.Value;

    public GeneratedToken GenerateToken(User user)
    {
        var jti = Guid.NewGuid().ToString();
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Name),
            new(JwtRegisteredClaimNames.Jti, jti)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new GeneratedToken(accessToken, jti, expiresAtUtc);
    }
}
