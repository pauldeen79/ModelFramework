namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CodeStatementBaseModelFactory : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    public override object CreateModel()
        => CreateBuilderFactories(
            GetOverrideModels(typeof(ICodeStatementBase)),
            new(Constants.Namespaces.DomainModels,
            nameof(CodeStatementBaseModelFactory),
            $"{Constants.Namespaces.Domain}.CodeStatementBase",
            $"{Constants.Namespaces.DomainModels}.CodeStatements",
            "CodeStatementBaseModel",
            $"{Constants.Namespaces.Domain}.CodeStatements"));
}
