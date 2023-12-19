namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsBaseRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    public override object CreateModel()
        => GetImmutableClasses(GetObjectsModelBaseTypes(), "ModelFramework.Objects");
}
