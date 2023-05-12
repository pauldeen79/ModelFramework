namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Objects/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Models");
}
