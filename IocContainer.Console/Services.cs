namespace IocContainer;

class Service1
{
    public Guid Id { get; } = Guid.NewGuid();

    public Service1(Service2 service2, Service3 service3)
    {
        Console.WriteLine($"Creating {nameof(Service1)}, id: {Id}");
        Console.WriteLine($"\t - with {nameof(Service2)}, id: {service2.Id}");
        Console.WriteLine($"\t\t - with {nameof(Service3)}, id: {service2.Service3.Id}");
        Console.WriteLine($"\t - with {nameof(Service3)}, id: {service3.Id}");
    }
}

class Service2
{
    public Guid Id { get; } = Guid.NewGuid();
    public Service3 Service3 { get; }

    public Service2(Service3 service3)
    {
        Console.WriteLine($"Creating {nameof(Service2)}, id: {Id}");
        Service3 = service3;
    }
}

class Service3
{
    public Guid Id { get; } = Guid.NewGuid();

    public Service3()
    {
        Console.WriteLine($"Creating {nameof(Service3)}, id: {Id}");
    }
}