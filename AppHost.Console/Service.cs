using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppHost.Console;

class Service
{
    private readonly IConfiguration _config;

    public Service(
        ILogger<Service> logger,
        IConfiguration config)
    {
        _config = config;
        logger.LogInformation("Creating a service...");
    }

    public int GetNumber() => _config.GetValue<int>("number");
}