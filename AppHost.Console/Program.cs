// Add Microsoft.Extensions.Hosting package
// Set appsettings.json file to copy always to output directory

using AppHost.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true) // Common for all, can be overwritten
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true) // Overwrites appsettings.json with more specific settings, TODO: ASPNETCORE_ENVIRONMENT?
    .AddEnvironmentVariables(); // Overwrite appsettings files with environment variables

var hostBuilder = Host.CreateDefaultBuilder();

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddSingleton<Service>();
});

var app = hostBuilder.Build();

var service = app.Services.GetService<Service>();
var logger = app.Services.GetService<ILogger<Program>>();
logger?.LogInformation("The number is: {Number}", service?.GetNumber());
