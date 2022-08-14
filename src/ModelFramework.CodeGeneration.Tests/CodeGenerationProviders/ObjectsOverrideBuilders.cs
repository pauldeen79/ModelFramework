namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsOverrideBuilders : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(ITypeBase), "ModelFramework.Objects");

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelOverrideTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Builders");
}
