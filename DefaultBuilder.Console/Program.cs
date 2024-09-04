using DefaultBuilder.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// The app can be started with different launchsettings.json profiles, use `dotnet run --launch-profile "Development"`
// Environment variables set in launchSettings.json override those set in the system environment.

// Default host builder:
// Sets the content root to the path returned by Directory.GetCurrentDirectory().
// Loads host configuration from: // TODO: test host configuration
//   - Environment variables prefixed with "DOTNET_"
//   - Command-line arguments
// Loads app configuration with default hierarchy from:
//   - command-line arguments
//   - environment variables
//   - secrets.json (external local system file with unique id file name in the project file, works when the app is in the Development environment)
//   - appsettings.{Environment}.json
//   - appsettings.json
// Adds logging providers:
//   - Console
//   - Debug
//   - EventSource
//   - EventLog (only when running on Windows)
// Enables scope (e.g. transient in singleton) and dependency validation when the Environment = "Development".

// Host.CreateApplicationBuilder is a lightweight version of a Host.CreateDefaultBuilder, it was introduced in .NET 7
// with more "linear" code instead of callbacks and provides a great control over the config. 
//

// TODO: test args

#if true
Console.WriteLine("Running default app builder");

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Services.Configure<Settings>(hostBuilder.Configuration.GetSection(nameof(Settings)));

#else // Default builder
Console.WriteLine("Running original default builder");

var hostBuilder = Host.CreateDefaultBuilder(args); 
hostBuilder.ConfigureServices((context, services) =>
{
    services.Configure<Settings>(context.Configuration.GetSection(nameof(Settings)));
});

#endif

var app = hostBuilder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
var settings = app.Services.GetRequiredService<IOptions<Settings>>().Value;
logger.LogInformation("Setting name: {Name}", settings.Name);

await app.StartAsync();
