namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class DatabaseCodeStatements : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ModelFramework.Database\\SqlStatements\\Builders";

    public override string DefaultFileName => "Builders.Generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetCodeStatementBuilderClasses(typeof(LiteralSqlStatement),
                                          typeof(ISqlStatement),
                                          typeof(ISqlStatementBuilder),
                                          "ModelFramework.Database.SqlStatements.Builders");
}
