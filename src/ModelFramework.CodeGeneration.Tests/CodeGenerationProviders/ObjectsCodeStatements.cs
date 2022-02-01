namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsCodeStatements : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ModelFramework.Objects\\CodeStatements\\Builders";

    public override string DefaultFileName => "Builders.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetCodeStatementBuilderClasses(typeof(LiteralCodeStatement),
                                          typeof(ICodeStatement),
                                          typeof(ICodeStatementBuilder),
                                          "ModelFramework.Objects.CodeStatements.Builders");
}
