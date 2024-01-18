using WebTextForum.Models;

namespace WebTextForum.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string? GetClaim(string claim);
    }
}
