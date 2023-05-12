namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsCodeStatementModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Objects/CodeStatements/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetCodeStatementBuilderClasses(typeof(LiteralCodeStatement),
                                          typeof(ICodeStatement),
                                          typeof(ICodeStatementModel),
                                          "ModelFramework.Objects.CodeStatements.Models");
}
