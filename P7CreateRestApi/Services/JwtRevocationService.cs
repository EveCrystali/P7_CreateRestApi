using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Services;

namespace Dot.Net.WebApi.Services;

public class JwtRevocationService(LocalDbContext context) : IJwtRevocationService
{
    private readonly LocalDbContext _context = context;

    public async Task RevokeUserTokensAsync(string userId)
    {
        var refreshTokens = _context.RefreshTokens.Where(rt => rt.UserId == userId).ToList();
        foreach (var token in refreshTokens)
        {
            token.IsRevoked = true;
        }
        await _context.SaveChangesAsync();
    }
}
