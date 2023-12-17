namespace Peruse.Api.Wttw.PlaywrightWindow;

public class SessionCleanupService(PlaywrightWindowContext playwrightWindowContext, ILogger<SessionCleanupService> logger) : IHostedService, IDisposable
{
private Timer _timer;

public Task StartAsync(CancellationToken cancellationToken)
{
    _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(16));
    return Task.CompletedTask;
}

private void DoWork(object state)
{
    var cutoff = DateTime.UtcNow.AddMinutes(-15);
    // foreach (var session in _browserSessions)
    // {
    //     if (!session.Value.IsExpired)
    //         continue;
    //     // Close the browser context and remove from the dictionary
    //     logger
    //     session.Value.BrowserContext.CloseAsync();
    //     _browserSessions.TryRemove(session.Key, out _);
    // }
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