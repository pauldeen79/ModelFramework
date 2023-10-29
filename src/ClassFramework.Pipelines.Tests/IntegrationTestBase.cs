namespace ClassFramework.Pipelines.Tests;

public abstract class IntegrationTestBase<T> : TestBase, IDisposable
    where T : class
{
    private bool disposedValue;
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    protected T CreateSut() => _scope.ServiceProvider.GetRequiredService<T>();

    protected IntegrationTestBase()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddPipelines()
            .BuildServiceProvider();
        _scope = _provider.CreateScope();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _scope.Dispose();
                _provider.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
