using OwnIocContainer.Library;

namespace OwnIocContainer.Tests;

public class ServiceCollectionTests
{
    [Fact]
    public void AddSingleton_WithInstance_ShouldRegisterService()
    {
        var services = new ServiceCollection();
        var myService = new MyService();

        services.AddSingleton(myService);
        var container = services.Build();

        var resolvedService = container.GetService<MyService>();

        Assert.Equal(myService, resolvedService);
    }

    [Fact]
    public void AddSingleton_WithoutInstance_ShouldRegisterService()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MyService>();
        var container = services.Build();

        var resolvedService = container.GetService<MyService>();

        Assert.NotNull(resolvedService);
    }

    [Fact]
    public void AddTransient_ShouldRegisterService()
    {
        var services = new ServiceCollection();

        services.AddTransient<MyService>();
        var container = services.Build();

        var resolvedService1 = container.GetService<MyService>();
        var resolvedService2 = container.GetService<MyService>();

        Assert.NotNull(resolvedService1);
        Assert.NotNull(resolvedService2);
        Assert.NotEqual(resolvedService1, resolvedService2);
    }

    [Fact]
    public void GetService_NotRegistered_ShouldThrowException()
    {
        var services = new ServiceCollection();
        var container = services.Build();

        Assert.Throws<InvalidOperationException>(() => container.GetService<MyService>());
    }
}