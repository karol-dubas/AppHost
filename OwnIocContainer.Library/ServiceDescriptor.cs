namespace OwnIocContainer.Library;

public class ServiceDescriptor
{
    public Type? ParentType { get; }
    public Type ChildType { get; }
    public object? Instance { get; set; }
    public ServiceLifetime Lifetime { get; }
    
    public ServiceDescriptor(Type childType, ServiceLifetime lifetime)
    {
        ChildType = childType;
        Lifetime = lifetime;
    }
    
    public ServiceDescriptor(object instance, ServiceLifetime lifetime)
    {
        ChildType = instance.GetType();
        Instance = instance;
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type parentType, Type childType, ServiceLifetime lifetime)
    {
        ParentType = parentType;
        ChildType = childType;
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type parentType, object instance, ServiceLifetime lifetime)
    {
        ParentType = parentType;
        Instance = instance;
        ChildType = instance.GetType();
        Lifetime = lifetime;
    }
}