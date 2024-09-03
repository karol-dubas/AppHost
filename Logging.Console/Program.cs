using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(static b =>
{
    b.AddSimpleConsole(o =>
    {
        o.SingleLine = true;
        o.IncludeScopes = true;
        o.TimestampFormat = "HH:mm:ss ";
    });
});

var logger = loggerFactory.CreateLogger<Program>();
using (logger.BeginScope("[Scope #1]"))
{
    logger.LogInformation("Number #1: {Number}", Random.Shared.Next(10));
}
logger.LogInformation("Number #2: {Number}", Random.Shared.Next(10));