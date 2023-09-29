namespace ClassFramework.Domain.Builders;

public partial class TypeBuilder
{
    public string GetFullName() => $"{Namespace.ToString().GetNamespacePrefix()}{Name}";
}

public abstract partial class TypeBuilder<TBuilder, TEntity> : TypeBuilder
{
    public TBuilder AddInterfaces(params System.Type[] types)
        => AddInterfaces(types.Select(x => x.FullName));

    public TBuilder AddInterfaces(IEnumerable<System.Type> interfaces)
        => AddInterfaces(interfaces.ToArray());
}
