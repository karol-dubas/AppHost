using Configuration.Console;
using Microsoft.Extensions.Configuration;

// IConfiguration type provides a unified view of the configuration data, using various configuration sources:
// - Settings files, such as appsettings.json
// - Environment variables
// - Azure Key Vault
// - Azure App Configuration
// - Command-line arguments
// - Custom providers, installed or created
// - Directory files
// - In-memory .NET objects
// - Third-party providers

const string envName = "Environment";

var config = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        [envName] = "Demo"
    })
    .AddJsonFile("appsettings.json")
    .Build();

Console.WriteLine($"In-memory '{envName}' value: {config[envName]}");

var settings = config.GetRequiredSection(nameof(Settings)).Get<Settings>();
Console.WriteLine($"appsetting's '{nameof(Settings.Name)}' value: {settings?.Name}");
