namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CodeStatementBaseBuilderFactory : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.DomainBuilders;

    public override object CreateModel()
        => CreateBuilderFactories(
            GetOverrideModels(typeof(ICodeStatementBase)),
            new(Constants.Namespaces.DomainBuilders,
            nameof(CodeStatementBaseBuilderFactory),
            $"{Constants.Namespaces.Domain}.CodeStatementBase",
            $"{Constants.Namespaces.DomainBuilders}.CodeStatements",
            "CodeStatementBaseBuilder",
            $"{Constants.Namespaces.Domain}.CodeStatements"));
}
