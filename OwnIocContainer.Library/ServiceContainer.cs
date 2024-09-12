using System.Diagnostics;
using System.Reflection;

namespace OwnIocContainer.Library;

public class ServiceContainer
{
    private readonly Dictionary<Type, ServiceDescriptor> _services;

    public ServiceContainer(Dictionary<Type, ServiceDescriptor> services)
    {
        _services = services;
    }

    public T GetService<T>() where T : class => (T)GetService(typeof(T));

    private object GetService(Type serviceType)
    {
        var descriptor = _services.GetValueOrDefault(serviceType);

        if (descriptor is null)
            throw new InvalidOperationException($"Service of type '{serviceType}' is not registered");

        switch (descriptor.Lifetime)
        {
            case ServiceLifetime.Singleton:
                descriptor.Instance ??= CreateInstance(descriptor);
                return descriptor.Instance;

            case ServiceLifetime.Transient:
                return CreateInstance(descriptor);

            default:
                throw new NotImplementedException();
        }
    }
    
    private object CreateInstance(ServiceDescriptor descriptor)
    {
        var constructors = descriptor.ChildType.GetConstructors();

        if (constructors.Length != 1)
            throw new InvalidOperationException("Registered type must have only one constructor");

        object[] ctorServiceParams = constructors
            .Single()
            .GetParameters()
            .Select(c => GetService(c.ParameterType))
            .ToArray();
        
        return Activator.CreateInstance(descriptor.ChildType, args: ctorServiceParams)
               ?? throw new UnreachableException();
    }
}