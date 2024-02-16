namespace ClassFramework.Pipelines;

public class NamedResultSetBuilder<T>
{
    private readonly List<NamedResult<Func<Result<T>>>> _resultset = new();

    public void Add(string name, Func<Result<T>> value) => _resultset.Add(new(name, value));

    public NamedResult<Result<T>>[] Build()
        => _resultset
            .Select(x => new NamedResult<Result<T>>(x.Name, x.Result()))
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();
}
