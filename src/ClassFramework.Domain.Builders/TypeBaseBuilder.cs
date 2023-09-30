namespace ClassFramework.Domain.Builders;

public partial class TypeBaseBuilder
{
    public string GetFullName() => $"{Namespace.GetNamespacePrefix()}{Name}";
}

public abstract partial class TypeBaseBuilder<TBuilder, TEntity> : TypeBaseBuilder
{
    public TBuilder AddInterfaces(params Type[] types)
        => AddInterfaces(types.Select(x => x.FullName));

    public TBuilder AddInterfaces(IEnumerable<Type> interfaces)
        => AddInterfaces(interfaces.ToArray());
}
