namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class TypeBaseModelFactory : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(ITypeBase)),
            new(Constants.Namespaces.DomainModels,
            nameof(TypeBaseModelFactory),
            $"{Constants.Namespaces.Domain}.TypeBase",
            $"{Constants.Namespaces.DomainModels}.Types",
            "TypeBaseModel",
            $"{Constants.Namespaces.Domain}.Types"));
}
