using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceScopeProvider;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>(); // Registers the worker as a singleton
builder.Services.AddScoped<Service>();

var host = builder.Build();
await host.RunAsync();
