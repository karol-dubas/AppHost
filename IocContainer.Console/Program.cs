using IocContainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// TODO: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection

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
    ValidateScopes = true, // Exclude scoped services injected in singletons
    ValidateOnBuild = true
});

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

// Services with a longer lifetime should not depend on services with a shorter lifetime.
// - Singleton services should only depend on singleton services.
// - Scoped services can depend on scoped and singleton services.
// - Transient services can depend on transient, scoped, and singleton services.

// Two services can't depend on each other, because it creates a circular dependency.

// Scoped service should never be injected in a singleton service,
// because it is created at a root level, and it won't work as intended.

// Transient service injected in a singleton service isn't recreated,
// because the singleton isn't recreated as well, and it works like a singleton.

// Use lazy factory or a service provider injection to create a new object or a scope.
