using Dot.Net.WebApi.Models;

namespace Dot.Net.WebApi.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string userName, string[] roles);

    RefreshToken GenerateRefreshToken(string userId);
}