using EmptyAppBuilder.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder
// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection

// Set appsettings.json file to copy always to output directory

// TODO: Add launchsettings.json
// The app can be started with different launchsettings.json profiles, use `dotnet run --launch-profile "Development"`
// Environment variables set in launchSettings.json override those set in the system environment.

string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
Console.WriteLine($"DOTNET_ENVIRONMENT: {environment}");

Console.WriteLine($"args: [{string.Join(", ", args)}]");

var settings = new HostApplicationBuilderSettings
{
    Args = args,
    DisableDefaults = true,
    ContentRootPath = Environment.CurrentDirectory,
    Configuration = new ConfigurationManager()
};

var builder = Host.CreateEmptyApplicationBuilder(settings);

// Each subsequent provider overrides the previous ones, creating a hierarchy built through chained methods.
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    //.AddJsonFile($"appsettings.{environment}.json", true, true) // TODO: add
    //.AddUserSecrets<Program>(true, true) // TODO: add
    .AddEnvironmentVariables()
    .AddEnvironmentVariables("MYAPP_")
    .AddCommandLine(args)
    ;

// Settings can be overwritten by env vars now, on windows cmd use:
// set Settings__Name="setting name env var"
// set MYAPP_Settings__Name="myapp setting name env var"

// Run the command without or with one of these argument options:
// dotnet run Settings:Name="cli setting"
// dotnet run /Settings:Name "cli setting"
// dotnet run --Settings:Name "cli setting"

builder.Services.AddOptions<Settings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(Settings)))
    .Validate(x => !string.IsNullOrWhiteSpace(x.Name), "The name can't be empty")
    .ValidateOnStart();

// Alternatively:
//builder.Services.Configure<Settings>(builder.Configuration.GetRequiredSection(nameof(Settings)));

builder.Services.AddTransient<Service>();

builder.Logging
    .AddConsole()
    .AddDebug();

using var appHost = builder.Build();

_ = RunPrintSettingLoop(appHost);

await appHost.RunAsync();

Task RunPrintSettingLoop(IHost host)
{
    return Task.Run(() =>
    {
        Console.WriteLine("Press 'O' to log the current settings");
        while (true)
        {
            if (Console.ReadKey(true).Key != ConsoleKey.O)
                return;
            
            // TODO: What is the difference between:
            // host.Services.CreateScope().ServiceProvider.GetRequiredService<>
            // host.Services.GetRequiredService<>
            // Does scope handle lifetimes, disposes, etc.?

            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<Service>()?.LogOptionsAsJson();
        }
    });
}
