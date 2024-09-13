using OwnIocContainer.Library;

namespace OwnIocContainer.Tests;

public class ServiceCollectionTests
{
    [Fact]
    public void GetService_NotRegistered_ShouldThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        var container = services.Build();
        
        // Act

        // Assert
        Assert.Throws<InvalidOperationException>(() => container.GetService<Service>());
    }
    
    [Fact]
    public void AddSingleton_WithMultipleInstance_ShouldRegisterLastService()
    {
        // Arrange
        var services = new ServiceCollection();
        var service1 = new Service();
        var service2 = new Service();

        // Act
        services.AddSingleton(service1);
        services.AddSingleton(service2);
        var container = services.Build();
        var resolvedService = container.GetService<Service>();
        
        // Assert
        Assert.Same(service2, resolvedService);
    }

    [Fact]
    public void AddSingleton_WithoutInstance_ShouldRegisterServiceAndReturnCreatedInstance()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSingleton<Service>();
        var container = services.Build();
        var resolvedService = container.GetService<Service>();
        
        // Assert
        Assert.NotNull(resolvedService);
    }

    [Fact]
    public void AddTransient_ShouldRegisterService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTransient<Service>();
        var container = services.Build();
        var resolvedService1 = container.GetService<Service>();
        var resolvedService2 = container.GetService<Service>();

        // Assert
        Assert.NotNull(resolvedService1);
        Assert.NotNull(resolvedService2);
        Assert.NotSame(resolvedService1, resolvedService2);
    }

    [Fact]
    public void AddTransient_WithInterface_ShouldRegisterServiceAsInterface()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTransient<IService, Service>();
        var container = services.Build();
        IService resolvedService1 = container.GetService<IService>();
        IService resolvedService2 = container.GetService<IService>();

        // Assert
        Assert.NotNull(resolvedService1);
        Assert.NotNull(resolvedService2);
        Assert.NotSame(resolvedService1, resolvedService2);
    }
    
    [Fact]
    public void GetService_WithConstructorInjection_ResolvesAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var childService = new ChildService();

        // Act
        services.AddTransient<IParentService, ParentService>();
        services.AddSingleton<IChildService, ChildService>(childService);
        services.AddSingleton<ChildService>();
        var container = services.Build();
        IParentService resolvedParentService1 = container.GetService<IParentService>();
        IParentService resolvedParentService2 = container.GetService<IParentService>();

        // Assert
        Assert.NotNull(resolvedParentService1);
        Assert.NotNull(resolvedParentService2);
        
        Assert.NotSame(resolvedParentService1, resolvedParentService2);
        
        Assert.Same(resolvedParentService1.ChildService, childService);
        Assert.Same(resolvedParentService2.ChildService, childService);
        
        Assert.Same(resolvedParentService1.ChildServiceImpl, resolvedParentService2.ChildServiceImpl);
    }
}