namespace KamSoft.BackgroundServiceExample;

public interface IBackgroundOperation
{
    Task Run(CancellationToken cancellationToken);
}

public class CustomOperation : IBackgroundOperation
{
    public async Task Run(CancellationToken cancellationToken)
    {
        Console.WriteLine("Operation is running...");
        await Task.Delay(1_000, cancellationToken);
        Console.WriteLine("Operation stops");
    }
}

public class BackgroundServiceExample<T>(T operation) : BackgroundService where T : class, IBackgroundOperation
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            await operation.Run(stoppingToken);
        }
    }
}

public static class RegistrationBackground
{
    public static IServiceCollection AddBackgroundService<T>(this IServiceCollection services) where T : class, IBackgroundOperation
    {
        services.AddScoped<T>();
        services.AddHostedService<BackgroundServiceExample<T>>();
        
        return services;
    }
}