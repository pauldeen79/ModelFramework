namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects";

    public override string DefaultFileName => "Entities.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetObjectsModelTypes(), "ModelFramework.Objects");
}
