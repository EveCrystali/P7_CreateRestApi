using Dot.Net.WebApi.Data;

namespace Dot.Net.WebApi.Services;

public class JwtRevocationService(LocalDbContext context) : IJwtRevocationService
{
    private readonly LocalDbContext _context = context;

    public async Task RevokeUserTokensAsync(string userId)
    {
        List<Models.RefreshToken> refreshTokens = _context.RefreshTokens.Where(rt => rt.UserId == userId).ToList();
        foreach (Models.RefreshToken? token in refreshTokens)
        {
            token.IsRevoked = true;
        }
        await _context.SaveChangesAsync();
    }
}