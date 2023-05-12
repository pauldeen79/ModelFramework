namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsOverrideModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Objects/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(ITypeBase), "ModelFramework.Objects");

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelOverrideTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Models");
}
