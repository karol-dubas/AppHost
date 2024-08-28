using EmptyAppBuilder.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var settings = new HostApplicationBuilderSettings
{
  Args = args,
  DisableDefaults = false,
  ContentRootPath = Environment.CurrentDirectory,
  Configuration = new ConfigurationManager()
};

var builder = Host.CreateEmptyApplicationBuilder(settings);

// Configure the builder...
builder.Configuration
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<Settings>(builder.Configuration.GetRequiredSection(nameof(Settings)));

using var host = builder.Build();

var config = host.Services.GetRequiredService<IOptions<Settings>>().Value;
Console.WriteLine(config?.Name);

// https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#environment-variable-configuration-provider

