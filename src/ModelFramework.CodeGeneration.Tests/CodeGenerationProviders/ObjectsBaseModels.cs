namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsBaseModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Objects/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelBaseTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Models");
}
