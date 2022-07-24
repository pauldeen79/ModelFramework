namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class DatabaseBuilders : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Database/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetDatabaseModelTypes(),
                                      "ModelFramework.Database",
                                      "ModelFramework.Database.Builders");
}
