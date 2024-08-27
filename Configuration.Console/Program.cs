using Microsoft.Extensions.Configuration;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration

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

var config = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Environment"] = "Demo"
    })
    .Build();

Console.WriteLine(config["Environment"]);
    