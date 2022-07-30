namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsBaseBuilders : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableEntityInheritance => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelBaseTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Builders");
}
