namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsOverrideRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableInheritance => true;
    protected override IClass? BaseClass => GetTypeBaseModel();

    public override object CreateModel()
        => GetImmutableClasses(GetObjectsModelOverrideTypes(), "ModelFramework.Objects");
}
