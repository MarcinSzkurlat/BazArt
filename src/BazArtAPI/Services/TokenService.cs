using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace BazArtAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.StageName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            int tokenExpireDays = Int32.Parse(_config["DaysExpireToken"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(tokenExpireDays),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
