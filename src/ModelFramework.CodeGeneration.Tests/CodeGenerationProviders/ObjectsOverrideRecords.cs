namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsOverrideRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(ITypeBase));

    public override object CreateModel()
        => GetImmutableClasses(GetObjectsModelOverrideTypes(), "ModelFramework.Objects");
}
