namespace OwnIocContainer.Library;

public class ServiceCollection
{
    private readonly Dictionary<Type, ServiceDescriptor> _services = [];

    public void AddSingleton<T>(T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        var service = new ServiceDescriptor(instance, ServiceLifetime.Singleton);
        _services[service.ChildType] = service;
    }

    public void AddSingleton<TService>() where TService : class
    {
        var service = new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton);
        _services[service.ChildType] = service;
    }
    
    public void AddSingleton<TParent, TChild>(TChild instance) where TChild : class, TParent
    {
        ArgumentNullException.ThrowIfNull(instance);
        
        var service = new ServiceDescriptor(
            typeof(TParent), instance, ServiceLifetime.Singleton);

        _services[service.ParentType!] = service;
    }
    
    public void AddSingleton<TParent, TChild>() where TChild : class, TParent
    {
        var service = new ServiceDescriptor(
            typeof(TParent), typeof(TChild), ServiceLifetime.Singleton);

        _services[service.ParentType!] = service;
    }
        
    public void AddTransient<TService>() where TService : class
    {
        var service = new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient);
        _services[service.ChildType] = service;
    }
    
    public void AddTransient<TParent, TChild>() where TChild : class, TParent
    {
        var service = new ServiceDescriptor(
            typeof(TParent), typeof(TChild), ServiceLifetime.Transient);

        _services[service.ParentType!] = service;
    }

    public ServiceContainer Build() => new(_services);
}