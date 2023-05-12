namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class DatabaseModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Database/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetDatabaseModelTypes(),
                                      "ModelFramework.Database",
                                      "ModelFramework.Database.Models");
}
