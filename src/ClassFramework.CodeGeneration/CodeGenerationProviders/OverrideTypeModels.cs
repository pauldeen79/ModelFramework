namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideTypeModels : ClassFrameworkModelClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainModels}/Types";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override ModelFramework.Objects.Contracts.IClass? BaseClass => CreateBaseclass(typeof(ITypeBase), Constants.Namespaces.Domain);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetOverrideModels(typeof(ITypeBase)),
            $"{Constants.Namespaces.Domain}.Types",
            $"{Constants.Namespaces.DomainModels}.Types");
}
