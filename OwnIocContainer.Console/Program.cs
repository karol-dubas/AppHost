using System.Collections.Frozen;
using OwnIocContainer.Console;

var services = new ServiceCollection();
//services.AddSingleton(new MyService());
services.AddSingleton<MyService>();

var container = services.Build();
Console.WriteLine(container.GetService<MyService>().Id);
Console.WriteLine(container.GetService<MyService>().Id);

namespace OwnIocContainer.Console
{
    public class ServiceCollection
    {
        // TODO: handle duplicate types
        private readonly List<ServiceDescriptor> _services = [];
    
        public void AddSingleton<T>(T instance) where T : class
        {
            ArgumentNullException.ThrowIfNull(instance);
        
            var service = new ServiceDescriptor(instance);
            _services.Add(service);
        }
    
        public void AddSingleton<T>() where T : class
        {
            var service = new ServiceDescriptor(typeof(T));
            _services.Add(service);
        }

        public ServiceContainer Build() => new(_services);
    }

    public class ServiceContainer
    {
        private readonly FrozenDictionary<Type, ServiceDescriptor> _services;

        public ServiceContainer(IEnumerable<ServiceDescriptor> services) 
            => _services = services.ToFrozenDictionary(s => s.Type, s => s);

        public T GetService<T>() where T : class
        {
            var serviceType = typeof(T);
            var descriptor = _services.GetValueOrDefault(serviceType);

            if (descriptor is null)
                throw new InvalidOperationException($"Service of type '{serviceType}' is not registered");

            descriptor.Instance ??= Activator.CreateInstance<T>();
        
            return (T)descriptor.Instance;
        }
    }
}