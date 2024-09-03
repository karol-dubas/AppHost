using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(static b =>
{
    b.AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Number: {Number}", Random.Shared.Next(10));
