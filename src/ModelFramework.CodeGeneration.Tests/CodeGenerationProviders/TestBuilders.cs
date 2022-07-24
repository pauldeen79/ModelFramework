namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestBuilders : TestCSharpClassBase
{
    public override string Path => "ModelFramework.Common/Test/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetTestModels(),
                                      "ModelFramework.Common.Test",
                                      "ModelFramework.Common.Test.Builders");

    public string GetNewCollectionTypeName() => NewCollectionTypeName;
}
