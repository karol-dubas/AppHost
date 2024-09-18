using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<TransientService>();
builder.Services.AddTransient<ChildTransientService>();
builder.Services.AddTransient<Func<TransientService>>(sp => () => new(sp.GetRequiredService<ChildTransientService>()));
builder.Services.AddScoped<ScopedService>();
builder.Services.AddSingleton<SingletonService>();

using var host = builder.Build();

ExemplifyDisposableScoping(host.Services, "Scope 1");
Console.WriteLine();

ExemplifyDisposableScoping(host.Services, "Scope 2");
Console.WriteLine();

await host.StartAsync();
Console.WriteLine();

static async void ExemplifyDisposableScoping(IServiceProvider services, string scope)
{
    Console.WriteLine($"{scope}:");

    //using var serviceScope = services.CreateScope();
    await using var serviceScope = services.CreateAsyncScope(); // Async scope handles IAsyncDisposable
    var provider = serviceScope.ServiceProvider;
    
    // Note that transient services are disposed when scope (or the container) is disposed.
    // Transient services shouldn't be disposed by the DI container, they should be disposed manually, using a factory. 
    var factory = provider.GetRequiredService<Func<TransientService>>();
    var service = factory();
    service.Dispose();

    _ = provider.GetRequiredService<TransientService>();
    _ = provider.GetRequiredService<ScopedService>();
    _ = provider.GetRequiredService<SingletonService>();
    
    _ = provider.GetRequiredService<TransientService>();
    _ = provider.GetRequiredService<ScopedService>();
    _ = provider.GetRequiredService<SingletonService>();
}
