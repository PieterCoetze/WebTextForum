using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebTextForum.Models;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _iconfiguration;
        private readonly IHttpContextAccessor _httpContext;

        public JwtService(IConfiguration iconfiguration, IHttpContextAccessor httpContext)
        {
            _iconfiguration = iconfiguration;
            _httpContext = httpContext;
        }

        public string GenerateToken(User user)
        {
            var issuer = _iconfiguration["Jwt:Issuer"];
            var audience = _iconfiguration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_iconfiguration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        public string? GetClaim(string claimName)
        {
            string? claim = null;

            if (_httpContext?.HttpContext?.User.Identity is ClaimsIdentity identity)
            {
                if (identity.Claims.Count() > 0)
                    claim = identity.FindFirst(claimName)?.Value;
            }

            return claim;
        }
    }
}
