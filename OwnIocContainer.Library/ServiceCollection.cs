namespace OwnIocContainer.Library;

public class ServiceCollection
{
    // TODO: handle duplicate types
    private readonly Dictionary<Type, ServiceDescriptor> _services = [];

    public void AddSingleton<T>(T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        var service = new ServiceDescriptor(instance, ServiceLifetime.Singleton);
        _services[service.Type] = service;
    }

    public void AddSingleton<T>() where T : class
    {
        var service = new ServiceDescriptor(typeof(T), ServiceLifetime.Singleton);
        _services[service.Type] = service;
    }
        
    public void AddTransient<T>() where T : class
    {
        var service = new ServiceDescriptor(typeof(T), ServiceLifetime.Transient);
        _services[service.Type] = service;
    }

    public ServiceContainer Build() => new(_services);
}