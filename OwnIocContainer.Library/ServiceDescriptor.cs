namespace OwnIocContainer.Library;

public class ServiceDescriptor
{
    public Type Type { get; }
    public object? Instance { get; set; } // TODO: public setter? Only singleton (split types)
    public ServiceLifetime Lifetime { get; }
    
    public ServiceDescriptor(object instance, ServiceLifetime lifetime)
    {
        Type = instance.GetType();
        Instance = instance;
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type type, ServiceLifetime lifetime)
    {
        Type = type;
        Lifetime = lifetime;
    }
}