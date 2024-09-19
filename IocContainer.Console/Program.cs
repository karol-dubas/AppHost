using IocContainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// Add services to the DI container
var services = new ServiceCollection();
services.AddTransient<Service1>();
services.AddScoped<Service2>();
services.AddSingleton<Service3>();
services.AddSingleton<Service3>();
services.TryAddSingleton<Service3>();

// Each next service registration with the same type overrides the previous one.
// Using TryAdd...() method registers only the first service of that type and
// ignores the following registrations.

var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
{
    // Exclude scoped services injected in singletons and resolving from a root provider
    ValidateScopes = true,
    ValidateOnBuild = true
});

// "Cannot resolve scoped service ... from root provider."
//scope.ServiceProvider.GetService<>();

Console.WriteLine("Scope 1:");
using var scope1 = serviceProvider.CreateScope();
Console.WriteLine("Run 1:");
scope1.ServiceProvider.GetRequiredService<Service1>();
Console.WriteLine("\nRun 2:");
scope1.ServiceProvider.GetRequiredService<Service1>();

Console.WriteLine("\nScope 2:");
Console.WriteLine("Run 1:");
using var scope2 = serviceProvider.CreateScope();
scope2.ServiceProvider.GetRequiredService<Service1>();

Console.WriteLine("\nService collection:");
serviceProvider.GetService<IEnumerable<Service3>>();

Console.WriteLine();

// Hierarchy:
// - Singleton
// - Scoped
// - Transient

// Singleton services should be thread-safe.

// Services with a longer lifetime should not depend on services with a shorter lifetime.
// - Singleton services should only depend on singleton services.
// - Scoped services can depend on scoped and singleton services.
// - Transient services can depend on transient, scoped, and singleton services.

// Two services can't depend on each other, because it creates a circular dependency.

// Scoped service without a scope works like a singleton.
