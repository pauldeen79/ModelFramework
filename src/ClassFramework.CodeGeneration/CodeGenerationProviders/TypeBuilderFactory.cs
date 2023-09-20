namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class TypeBuilderFactory : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.DomainBuilders;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(IType)),
            new(Constants.Namespaces.DomainBuilders,
            nameof(TypeBuilderFactory),
            $"{Constants.Namespaces.Domain}.Type",
            $"{Constants.Namespaces.DomainBuilders}.Types",
            "TypeBuilder",
            $"{Constants.Namespaces.Domain}.Types"));
}
