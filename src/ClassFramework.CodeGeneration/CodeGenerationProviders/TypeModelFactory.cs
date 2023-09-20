namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class TypeModelFactory : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(IType)),
            new(Constants.Namespaces.DomainModels,
            nameof(TypeModelFactory),
            $"{Constants.Namespaces.Domain}.Type",
            $"{Constants.Namespaces.DomainModels}.Types",
            "TypeModel",
            $"{Constants.Namespaces.Domain}.Types"));
}
