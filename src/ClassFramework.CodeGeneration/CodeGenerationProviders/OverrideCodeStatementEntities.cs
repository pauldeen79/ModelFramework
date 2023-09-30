namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideCodeStatementEntities : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Domain}/CodeStatements";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override ModelFramework.Objects.Contracts.IClass? BaseClass => CreateBaseclass(typeof(ICodeStatementBase), Constants.Namespaces.Domain);

    public override object CreateModel()
        => GetImmutableClasses(GetOverrideModels(typeof(ICodeStatementBase)), $"{Constants.Namespaces.Domain}.CodeStatements");
}
