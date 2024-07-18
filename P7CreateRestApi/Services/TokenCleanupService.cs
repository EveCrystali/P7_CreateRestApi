using Dot.Net.WebApi.Data;

public class TokenCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Starts the hosted service.
    /// This method is called when the hosted service is started. It creates a new timer that
    /// runs the <see cref="CleanUpExpiredTokens"/> method every hour.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that indicates when the service should stop.</param>
    /// <returns>A task that represents the start operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a new timer that runs the CleanUpExpiredTokens method every hour.
        // The first argument is the method to be executed.
        // The second argument is the state object that is passed to the method.
        // The third argument is the initial delay before the first execution.
        // The fourth argument is the period between subsequent executions.
        _timer = new Timer(
            callback: CleanUpExpiredTokens,
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromHours(1)
        );

        // Return a completed task because starting the service is asynchronous and does not need to be awaited.
        _timer = new Timer(CleanUpExpiredTokens, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Cleans up expired refresh tokens from the database.
    /// This method is executed periodically by the <see cref="TokenCleanupService"/>.
    /// </summary>
    /// <param name="state">The state object passed to the method.</param>
    private void CleanUpExpiredTokens(object state)
    {
        // Create a new scope for database operations.
        using IServiceScope scope = _scopeFactory.CreateScope();

        // Get the instance of the LocalDbContext.
        LocalDbContext context = scope.ServiceProvider.GetRequiredService<LocalDbContext>();

        // Get the list of expired refresh tokens from the database.
        List<Dot.Net.WebApi.Models.RefreshToken> expiredTokens = context.RefreshTokens
            .Where(rt => rt.ExpiryDate < DateTime.UtcNow)
            .ToList();

        // Remove the expired tokens from the database.
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