using Application.exceptions;
using Infrastructure.authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApi.Extentions;

public static class ApiExtentions
{
    public static IServiceCollection AddBearerApiAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration.GetSection(nameof(JwtOptions)).GetSection("SecretKey").Value ?? "";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static Guid GetUserIdFromToken(this HttpContext context)
    {
        var claimType = "userId";
        var tokenExists = context.Request.Headers.TryGetValue("Authorization", out var authHeader);

        if (!tokenExists)
        {
            throw new ForbiddenException("Authorization failed");
        }

        var jwtToken = authHeader.ToString().Substring("Bearer ".Length).Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;
        var userId = jsonToken?.Claims?.FirstOrDefault(c => c.Type == claimType)?.Value;

        return userId == null ? Guid.Empty : Guid.Parse(userId);
    }
}
