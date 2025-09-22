using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ThorAPI.Models;

namespace ThorAPI.Utils;

public static class Jwt {
    private static readonly string SecretKey = "aslkdjnaslkjdnalkdjnalksjdnakdjnalksdjniufrebouiebrgfoiej flwkjd nalksjdnalksjdnalksjdnw9dn29dnoeifnlskjdlk"; 
    private static readonly byte[] Key = Encoding.UTF8.GetBytes(SecretKey);

    public static string GenerateToken(Usuario user, int expireMinutes = 60)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[] {
            new Claim("Id", user.Id.ToString()),
            new Claim("Cpf", user.Cpf),
            new Claim("Nome", user.Nome),
            new Claim("Email", user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static Usuario? ValidateToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        try {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            }, out _);

            var id = int.Parse(principal.FindFirst("Id")?.Value ?? "0");
            var cpf = principal.FindFirst("Cpf")?.Value ?? "";
            var nome = principal.FindFirst("Nome")?.Value ?? "";
            var email = principal.FindFirst("Email")?.Value ?? "";

            return new Usuario {
                Id = id,
                Cpf = cpf,
                Nome = nome,
                Email = email
            };
        }
        catch
        {
            return null;
        }
    }
}

