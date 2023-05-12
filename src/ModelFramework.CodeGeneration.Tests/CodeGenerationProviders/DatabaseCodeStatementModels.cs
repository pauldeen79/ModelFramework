namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class DatabaseCodeStatementModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Database/SqlStatements/Models";
    public override string DefaultFileName => "Models.Generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetCodeStatementBuilderClasses(typeof(LiteralSqlStatement),
                                          typeof(ISqlStatement),
                                          typeof(ISqlStatementModel),
                                          "ModelFramework.Database.SqlStatements.Models");
}
