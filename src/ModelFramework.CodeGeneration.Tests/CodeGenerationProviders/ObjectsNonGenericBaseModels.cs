namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsNonGenericBaseModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Objects/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override string FileNameSuffix => ".nongeneric.template.generated";

    public override object CreateModel()
        => GetImmutableNonGenericBuilderClasses(GetObjectsModelBaseTypes(),
                                                "ModelFramework.Objects",
                                                "ModelFramework.Objects.Models");
}
