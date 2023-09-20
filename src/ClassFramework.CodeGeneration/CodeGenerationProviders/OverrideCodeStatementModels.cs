namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideCodeStatementModels : ClassFrameworkModelClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainModels}/CodeStatements";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override ModelFramework.Objects.Contracts.IClass? BaseClass => CreateBaseclass(typeof(ICodeStatement), Constants.Namespaces.Domain);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetOverrideModels(typeof(ICodeStatement)),
            $"{Constants.Namespaces.Domain}.CodeStatements",
            $"{Constants.Namespaces.DomainModels}.CodeStatements");
}
