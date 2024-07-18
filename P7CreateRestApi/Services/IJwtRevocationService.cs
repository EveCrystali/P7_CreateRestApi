namespace Dot.Net.WebApi.Services;

public interface IJwtRevocationService
{
    Task RevokeUserTokensAsync(string userId);
}