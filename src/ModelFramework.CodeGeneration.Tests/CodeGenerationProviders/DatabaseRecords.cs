namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class DatabaseRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Database";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database");
}
