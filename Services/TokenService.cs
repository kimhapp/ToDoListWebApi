using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Services
{
    // Using TokenService over static method to generate token
    // As config will be injected through DI instead of passing it every time it is called
    public interface ITokenService
    {
        string GenerateToken(User user);
    }

    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string GenerateToken(User user)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["JWTKEY"]!));

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims = 
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            ];

            JwtSecurityToken token = new(
                issuer: configuration["JWTISSUER"],
                audience: configuration["JWTAUDIENCE"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}