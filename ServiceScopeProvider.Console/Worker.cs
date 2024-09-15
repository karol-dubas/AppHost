using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceScopeProvider;

public class Worker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var numberService = scope.ServiceProvider.GetRequiredService<Service>();
            Console.WriteLine($"Id: {numberService.Id}");
            var numberService2 = scope.ServiceProvider.GetRequiredService<Service>();
            Console.WriteLine($"Id: {numberService2.Id}");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
