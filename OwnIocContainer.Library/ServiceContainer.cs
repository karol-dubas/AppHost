namespace OwnIocContainer.Library;

public class ServiceContainer
{
    private readonly Dictionary<Type, ServiceDescriptor> _services;

    public ServiceContainer(Dictionary<Type, ServiceDescriptor> services)
    {
        _services = services;
    }

    public T GetService<T>() where T : class
    {
        var serviceType = typeof(T);
        var descriptor = _services.GetValueOrDefault(serviceType);

        if (descriptor is null)
            throw new InvalidOperationException($"Service of type '{serviceType}' is not registered");

        if (descriptor.Lifetime is ServiceLifetime.Singleton && descriptor.Instance is not null)
            return (T)descriptor.Instance;

        switch (descriptor.Lifetime)
        {
            case ServiceLifetime.Singleton:
                descriptor.Instance = Activator.CreateInstance<T>();
                return (T)descriptor.Instance;

            case ServiceLifetime.Transient:
                return Activator.CreateInstance<T>();

            default:
                throw new NotImplementedException();
        }
    }
}