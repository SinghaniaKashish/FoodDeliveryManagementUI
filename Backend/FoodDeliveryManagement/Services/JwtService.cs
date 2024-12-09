using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodDeliveryManagement.Services
{
    public class JwtService
    {
        private readonly string key;
        private readonly string issuer;
        private readonly string audience;

        public JwtService(IConfiguration configuration)
        {
            key = configuration["Jwt:key"];
            issuer = configuration["Jwt:issuer"];
            audience = configuration["Jwt:audience"];
        }

        public string GenerateToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

