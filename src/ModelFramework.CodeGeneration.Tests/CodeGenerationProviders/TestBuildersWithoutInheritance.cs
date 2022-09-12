namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestBuildersWithoutInheritance : TestCSharpClassBaseWithoutInheritance
{
    public override string Path => "ModelFramework.Common.Tests/Test/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(TestCSharpClassBase.CreateTestModels(),
                                      "ModelFramework.Common.Tests.Test",
                                      "ModelFramework.Common.Tests.Test.Builders");

    public string GetNewCollectionTypeName() => BuilderClassCollectionType.WithoutGenerics();
}
