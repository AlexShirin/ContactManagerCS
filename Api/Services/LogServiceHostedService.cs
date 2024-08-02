namespace ContactManagerCS.Services;

public class LogServiceHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public LogServiceHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
            logService.Start();
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}