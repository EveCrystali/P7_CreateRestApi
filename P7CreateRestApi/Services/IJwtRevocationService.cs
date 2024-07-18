public interface IJwtRevocationService
{
    Task RevokeUserTokensAsync(string userId);
}
