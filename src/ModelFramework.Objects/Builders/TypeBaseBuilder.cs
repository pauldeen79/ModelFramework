namespace ModelFramework.Objects.Builders;

public partial class TypeBaseBuilder<TBuilder, TEntity>
{
    public TBuilder AddGeneratedCodeAttribute(string generatorName, string generatorVersion)
        => AddAttributes(new AttributeBuilder().ForCodeGenerator(generatorName, generatorVersion));

    public TBuilder AddUsings(params string[] usings)
        => AddMetadata(usings.Select(x => new MetadataBuilder().WithName(MetadataNames.CustomUsing).WithValue(x)));

    public TBuilder AddNamespacesToAbbreviate(params string[] namespacesToAbbreviate)
        => AddMetadata(namespacesToAbbreviate.Select(x => new MetadataBuilder().WithName(MetadataNames.NamespaceToAbbreviate).WithValue(x)));

    public TBuilder AddInterfaces(params Type[] types)
        => AddInterfaces(types.Select(x => x.FullName));

    public TBuilder AddInterfaces(IEnumerable<Type> interfaces)
        => AddInterfaces(interfaces.ToArray());

    public TBuilder WithCustomValidationCode(string customValidationCode)
        => AddMetadata(MetadataNames.CustomValidateCode, customValidationCode);

    public override string ToString() => string.IsNullOrEmpty(Namespace)
        ? Name
        : $"{Namespace}.{Name}";
}

public partial class TypeBaseBuilder
{
    public string GetFullName() => $"{Namespace.GetNamespacePrefix()}{Name}";
}
