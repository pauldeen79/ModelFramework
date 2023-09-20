namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CodeStatementModelFactory : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(ICodeStatement)),
            new(Constants.Namespaces.DomainModels,
            nameof(CodeStatementModelFactory),
            $"{Constants.Namespaces.Domain}.CodeStatement",
            $"{Constants.Namespaces.DomainModels}.CodeStatements",
            "CodeStatementModel",
            $"{Constants.Namespaces.Domain}.CodeStatements"));
}
