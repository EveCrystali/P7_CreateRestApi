using Dot.Net.WebApi.Data;

public class TokenCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CleanUpExpiredTokens, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private void CleanUpExpiredTokens(object state)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        LocalDbContext context = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
        List<Dot.Net.WebApi.Models.RefreshToken> expiredTokens = context.RefreshTokens.Where(rt => rt.ExpiryDate < DateTime.UtcNow).ToList();
        context.RefreshTokens.RemoveRange(expiredTokens);
        context.SaveChanges();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}