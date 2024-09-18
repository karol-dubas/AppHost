public sealed class TransientService : IDisposable
{
    public TransientService(ChildTransientService service) { }

    public void Dispose() => Console.WriteLine($"Disposing {nameof(TransientService)}...");
}

public sealed class ChildTransientService : IDisposable
{
    public void Dispose() => Console.WriteLine($"Disposing {nameof(ChildTransientService)}...");
}

public sealed class ScopedService : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        Console.WriteLine($"Disposing {nameof(ScopedService)} asynchronously...");
        return ValueTask.CompletedTask;
    }
}

public sealed class SingletonService : IDisposable
{
    public void Dispose() => Console.WriteLine($"Disposing {nameof(SingletonService)}...");
}