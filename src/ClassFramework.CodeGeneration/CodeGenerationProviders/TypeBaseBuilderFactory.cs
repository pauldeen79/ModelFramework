namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class TypeBaseBuilderFactory : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.DomainBuilders;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(ITypeBase)),
            new(Constants.Namespaces.DomainBuilders,
            nameof(TypeBaseBuilderFactory),
            $"{Constants.Namespaces.Domain}.TypeBase",
            $"{Constants.Namespaces.DomainBuilders}.Types",
            "TypeBaseBuilder",
            $"{Constants.Namespaces.Domain}.Types"));
}
