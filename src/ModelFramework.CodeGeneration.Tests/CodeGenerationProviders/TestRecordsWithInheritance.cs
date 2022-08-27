namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestRecordsWithInheritance : TestCSharpClassBaseWithInheritance
{
    public override string Path => "ModelFramework.Common.Tests/Test";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(TestCSharpClassBase.CreateTestModels(), "ModelFramework.Common.Tests.Test");
}
