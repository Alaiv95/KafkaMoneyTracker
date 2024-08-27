using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.authentication;

public class JwtTokenFactory : ITokenFactory
{
    private readonly JwtOptions _options;

    public JwtTokenFactory(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(Guid userId, string userEmail)
    {
        Claim[] claims = new Claim[] { new("userId", userId.ToString()), new("email", userEmail) };

        var signInCredential = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signInCredential,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
