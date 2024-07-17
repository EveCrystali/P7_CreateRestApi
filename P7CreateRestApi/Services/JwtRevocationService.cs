
using Dot.Net.WebApi.Data;

namespace Dot.Net.WebApi.Services;

public interface IJwtRevocationService
{
    Task RevokeUserTokensAsync(string userId);
}

public class JwtRevocationService : IJwtRevocationService
{
    private readonly LocalDbContext _context;

    public JwtRevocationService(LocalDbContext context)
    {
        _context = context;
    }

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
