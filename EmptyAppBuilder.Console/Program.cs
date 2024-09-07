using EmptyAppBuilder.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder
// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
// TODO: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-8.0#lsj

// The launch profiles are configuration sets for running an application, with configured
// environment variables, command line arguments, etc.
// Environment variables set in launchsettings.json override those set in the system environment.
// To start the app with different profiles, edit ./Properties/launchsettings.json and, use:
// dotnet run --launch-profile <profile_name>

// TODO: print all DOTNET_ env vars
string? dotnetEnv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
Console.WriteLine($"DOTNET_ENVIRONMENT: {dotnetEnv}");
Console.WriteLine($"args: [{string.Join(", ", args)}]");

// Host settings can be configured directly with HostApplicationBuilderSettings
// or with environment variables matching Microsoft.Extensions.Hosting.HostDefaults field values.

var configManager = new ConfigurationManager();
configManager.AddEnvironmentVariables(prefix: "DOTNET_");

var settings = new HostApplicationBuilderSettings
{
    Args = args,
    DisableDefaults = true,
    Configuration = configManager
};

// Empty builder applies:
// - IConfigurationBuilder.AddCommandLine (with passed args in host settings param) 
// - setting's configuration base directory path (can be modified in host settings)
var appBuilder = Host.CreateEmptyApplicationBuilder(settings);

// Environment by default is set to a "Production", or when the name is invalid.
// The value is logged when the app starts.
string currentEnv = appBuilder.Environment.EnvironmentName;

// Configure app config.
// Each following provider overrides the previous ones, creating a hierarchy built through chained methods.
// Set appsettings.json file to copy always to output directory.
appBuilder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{currentEnv}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddEnvironmentVariables("MYAPP_")
    ;

// TODO: move to launchsettings.json
// Settings can be overwritten by env vars now, on windows cmd use:
// set Settings__Name="setting name env var"
// set MYAPP_Settings__Name="myapp setting name env var"

// Run the command without or with one of these argument options:
// dotnet run Settings:Name="cli setting"
// dotnet run /Settings:Name "cli setting"
// dotnet run --Settings:Name "cli setting"

appBuilder.Services.AddOptions<Settings>()
    .Bind(appBuilder.Configuration.GetRequiredSection(nameof(Settings)))
    .Validate(x => !string.IsNullOrWhiteSpace(x.Name), "The name can't be empty")
    .ValidateOnStart();

// Alternatively:
//builder.Services.Configure<Settings>(builder.Configuration.GetRequiredSection(nameof(Settings)));

appBuilder.Services.AddTransient<SettingsService>();

appBuilder.Logging
    .AddConsole()
    .AddDebug();

// TODO: check other default settings and configurations

// Build automatically registers:
// - IHostApplicationLifetime (post-startup and graceful shutdown tasks)
// - IHostLifetime (when the host starts and when it stops)
// - IHostEnvironment
using var appHost = appBuilder.Build();

_ = RunMenuLoop(appHost);

await appHost.RunAsync();

Task RunMenuLoop(IHost host)
{
    return Task.Run(() =>
    {
        Console.WriteLine("Press 'O' to log the current settings");
        Console.WriteLine("Press 'Q' to exit the app");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            
            if (key == ConsoleKey.O)
            {
                using var scope = host.Services.CreateScope();
                scope.ServiceProvider.GetRequiredService<SettingsService>()?.LogSettingsAsJson();
            }
            else if (key == ConsoleKey.Q)
            {
                using var scope = host.Services.CreateScope();
                scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>().StopApplication();
            }

            // TODO: What is the difference between:
            // host.Services.CreateScope().ServiceProvider.GetRequiredService<>
            // host.Services.GetRequiredService<>
            // Does scope handle lifetimes, disposes, etc.?
        }
    });
}
