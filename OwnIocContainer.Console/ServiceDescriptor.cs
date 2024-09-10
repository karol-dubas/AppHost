namespace OwnIocContainer.Console;

public class ServiceDescriptor
{
    public Type Type { get; }
    public object? Instance { get; set; } // TODO: public set?
    
    public ServiceDescriptor(object instance)
    {
        Type = instance.GetType();
        Instance = instance;
    }

    public ServiceDescriptor(Type type)
    {
        Type = type;
    }
}