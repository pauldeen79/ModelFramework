namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestRecords : TestCSharpClassBase
{
    public override string Path => "ModelFramework.Common.Tests/Test";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetTestModels(), "ModelFramework.Common.Tests.Test");
}
