namespace OwnIocContainer.Tests;

public interface IParentService
{
    Guid Id { get; }
    IChildService ChildService { get; }
    ChildService ChildServiceImpl { get; }
}

public class ParentService : IParentService
{
    public ParentService(IChildService childService, ChildService childServiceImpl)
    {
        ChildService = childService;
        ChildServiceImpl = childServiceImpl;
    }

    public IChildService ChildService { get; }
    public ChildService ChildServiceImpl { get; }

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