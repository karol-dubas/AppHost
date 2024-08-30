using AppHost.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Add Microsoft.Extensions.Hosting package
// Set appsettings.json file to copy always to output directory

// The app can be started with different launchsettings.json profiles, use `dotnet run --launch-profile "Development"`
// Environment variables set in launchSettings.json override those set in the system environment.

// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder
// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection

string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
Console.WriteLine($"DOTNET_ENVIRONMENT: {environment}");

// Default builder configures DI container, logging and configuration
var hostBuilder = Host.CreateDefaultBuilder(args); // TODO: test args

hostBuilder.ConfigureLogging(builder =>
{
    builder.ClearProviders();
    builder.AddDebug();
    builder.AddConsole();
});

hostBuilder.ConfigureHostConfiguration(builder =>
{
    builder.Sources.Clear();
    
    // it is done by default host builder, but just for a demo
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Common for all, it can be overwritten
        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Overwrites appsettings.json with more specific settings
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables("DOTNET_"); // Overwrite appsettings files with environment variables

    // TODO: it ignores commenting secrets and env vars, why is that?
    
    // Default hierarchy:
    // - command-line arguments
    // - environment variables
    // - secrets.json (external local system file with unique id file name in the project file, works when the app is in the Development environment)
    // - appsettings.{Environment}.json
    // - appsettings.json
    
    // TODO: is hierarchy changed with builder Add.. methods?
});

hostBuilder.ConfigureServices((context, services) =>
{
    services.Configure<Settings>(context.Configuration.GetSection(nameof(Settings)));
    services.AddScoped<Service>();
});

var app = hostBuilder.Build();

// TODO: What is the difference between:
// app.Services.CreateScope().ServiceProvider.GetRequiredService<>
// app.Services.GetService<>
// Does scope handle lifetimes, disposes, etc.?

Task.Run(() =>
{
    Console.WriteLine("Press 'O' to log the current settings");
    while (true)
    {
        if (Console.ReadKey(true).Key != ConsoleKey.O)
            return;

        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var service = scope.ServiceProvider.GetRequiredService<Service>();
            logger?.LogInformation(service?.GetOptionsAsJson());
        }
    } 
});

await app.RunAsync();