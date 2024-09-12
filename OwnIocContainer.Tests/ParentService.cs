namespace OwnIocContainer.Tests;

public interface IParentService
{
    Guid Id { get; }
    IChildService ChildService { get; }
}

public class ParentService : IParentService
{
    public ParentService(IChildService childService)
    {
        ChildService = childService;
    }

    public IChildService ChildService { get; }

    public Guid Id { get; } = Guid.NewGuid();
}

public interface IChildService
{
    Guid Id { get; }
}

public class ChildService : IChildService
{
    public Guid Id { get; } = Guid.NewGuid();
}