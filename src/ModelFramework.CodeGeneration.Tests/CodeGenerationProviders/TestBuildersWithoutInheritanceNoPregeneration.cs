namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestBuildersWithoutInheritanceNoPregeneration : TestCSharpClassBaseWithoutInheritance
{
    public override string Path => "ModelFramework.Common.Tests/Test/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;
    protected override Type BuilderClassCollectionType => typeof(IEnumerable<>);

    public override object CreateModel()
        => GetImmutableBuilderClasses(new[] { typeof(IChild).ToClass(CreateClassSettings()) },
                                      "ModelFramework.Common.Tests.Test",
                                      "ModelFramework.Common.Tests.Test.Builders");

    public string GetNewCollectionTypeName() => BuilderClassCollectionType.WithoutGenerics();
}
