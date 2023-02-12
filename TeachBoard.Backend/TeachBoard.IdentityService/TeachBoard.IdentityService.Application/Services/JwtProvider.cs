using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Services;

public class JwtProvider
{
    private readonly JwtConfiguration _configuration;

    public JwtProvider(JwtConfiguration configuration)
        => _configuration = configuration;

    public string GenerateUserJwt(User user)
    {
        // Добавляю клаймы в токен
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "auth"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new Claim("id", user.Id.ToString()),
        };
        // Добавляю роли юзера в клаймы токена
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        // Ключ подписи
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Формирую токен и возвращаю его
        var token = new JwtSecurityToken(
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            expires: DateTime.UtcNow + _configuration.Expire,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}