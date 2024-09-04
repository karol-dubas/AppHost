using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Add services to the DI container including logging
var services = new ServiceCollection();
services.AddLogging(b => b.AddConsole());
services.AddSingleton<Service>();

var serviceProvider = services.BuildServiceProvider();

// It automatically creates the required logger
var service = serviceProvider.GetRequiredService<Service>();
service.DoWork();

class Service(ILogger<Service> logger)
{
    public void DoWork() => logger.LogInformation("Hello world");
}