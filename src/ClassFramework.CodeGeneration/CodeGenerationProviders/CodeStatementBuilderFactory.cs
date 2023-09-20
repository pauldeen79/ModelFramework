namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CodeStatementBuilderFactory : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.DomainBuilders;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(ICodeStatement)),
            new(Constants.Namespaces.DomainBuilders,
            nameof(CodeStatementBuilderFactory),
            $"{Constants.Namespaces.Domain}.CodeStatement",
            $"{Constants.Namespaces.DomainBuilders}.CodeStatements",
            "CodeStatementBuilder",
            $"{Constants.Namespaces.Domain}.CodeStatements"));
}
