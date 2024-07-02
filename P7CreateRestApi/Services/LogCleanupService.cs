
namespace Dot.Net.WebApi.Services;

public class LogCleanupService : IHostedService, IDisposable
{
    private readonly ILogger<LogCleanupService> _logger;
    private readonly IConfiguration _configuration;
    private Timer? _timer;
    private bool _disposed = false;

    public static readonly string _logFilePath = Path.Combine("logs", $"Log-{DateTime.Now:yyyy-MM-dd}.txt");

    public LogCleanupService(ILogger<LogCleanupService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Log Cleanup Service is starting.");
        _timer = new Timer(DeleteOldLogs, null, TimeSpan.Zero, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Log Cleanup Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void DeleteOldLogs(object? state)
    {
        int logRetentionDays = _configuration.GetValue<int>("LogSettings:LogRetentionDays");

        string messageLog = $"Log cleanup service is deleting old logs. Log retention days: {logRetentionDays}";
        _logger.LogInformation(messageLog);

        string logDirectory = "logs";

        if (Directory.Exists(logDirectory))
        {
            var logFiles = Directory.GetFiles(logDirectory, "*.txt");
            foreach (var logFile in logFiles)
            {
                var creationTime = File.GetCreationTime(logFile);
                if (creationTime < DateTime.Now.AddDays(-logRetentionDays))
                {
                    File.Delete(logFile);
                    string messageLog2 = $"Deleted log file: {logFile}";
                    _logger.LogInformation(messageLog2);
                }
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _timer?.Dispose();
            }

            // Dispose unmanaged resources (if any)

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
