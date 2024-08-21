using AppHost.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Add Microsoft.Extensions.Hosting package
// Set appsettings.json file to copy always to output directory

// The app can be started with different launchsettings.json profiles, use `dotnet run --launch-profile "Development"`

// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder

Console.WriteLine($"Current environment: {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}");

var hostBuilder = Host.CreateDefaultBuilder();

hostBuilder.ConfigureHostConfiguration(builder =>
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true) // Common for all, can be overwritten
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", true, true) // Overwrites appsettings.json with more specific settings
        .AddEnvironmentVariables(); // Overwrite appsettings files with environment variables
    
    // Hierarchy:
    // - environment variables & console arguments
    // - secrets.json (external local system file, unique id is included in project file)
    // - appsettings.{Environment}.json
    // - appsettings.json
});

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddSingleton<Service>();
});

var app = hostBuilder.Build();

var logger = app.Services.GetService<ILogger<Program>>();

// TODO: What is the difference?
// 1
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<Service>();
    service.GetName();
    logger?.LogInformation("The name is: {Name}", service?.GetName());
}

// 2
{
    var service = app.Services.GetService<Service>();
    logger?.LogInformation("The name is: {Name}", service?.GetName());
}

//await app.RunAsync();