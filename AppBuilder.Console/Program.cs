using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Host.CreateApplicationBuilder is a lightweight version of a Host.CreateDefaultBuilder introduced in .NET 7
// with more "linear" code instead of callbacks and provides a great control over the config. 
var builder = Host.CreateApplicationBuilder();

// Configure the builder...

using var host = builder.Build();

var config = host.Services.GetRequiredService<IConfiguration>();
Console.WriteLine($"KeyOne = {config.GetValue<int>("KeyOne")}");
Console.WriteLine($"KeyThree:Message = {config.GetValue<string>("KeyThree:Message")}");

//await host.RunAsync();