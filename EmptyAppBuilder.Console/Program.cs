using EmptyAppBuilder.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var settings = new HostApplicationBuilderSettings
{
  Args = args,
  DisableDefaults = true,
  ContentRootPath = Environment.CurrentDirectory,
  Configuration = new ConfigurationManager()
};

var builder = Host.CreateEmptyApplicationBuilder(settings);

Console.WriteLine($"args: [{string.Join(", ", args)}]");

// Each subsequent provider overrides the previous ones, creating a hierarchy built through chained methods.
builder.Configuration
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddEnvironmentVariables()
  .AddEnvironmentVariables(prefix: "MYAPP_")
  .AddCommandLine(args)
  ;

// Settings can be overwritten by env vars now, on windows cmd use:
// set Settings__Name="setting name env var"
// set MYAPP_Settings__Name="myapp setting name env var"

// Run the command with one of these argument options:
// dotnet run Settings:Name="cli setting"
// dotnet run /Settings:Name "cli setting"
// dotnet run --Settings:Name "cli setting"

builder.Services.AddOptions<Settings>()
  .Bind(builder.Configuration.GetRequiredSection(nameof(Settings)))
  .Validate(x => !string.IsNullOrWhiteSpace(x.Name), "The name can't be empty")
  .ValidateOnStart();

// Alternatively:
//builder.Services.Configure<Settings>(builder.Configuration.GetRequiredSection(nameof(Settings)));

builder.Logging.AddConsole();

using var host = builder.Build();

var config = host.Services.GetRequiredService<IOptions<Settings>>().Value;
var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Name: {Name}", config?.Name);

//await host.RunAsync();
