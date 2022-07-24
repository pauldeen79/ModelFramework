namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonBuilders : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Common/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                      "ModelFramework.Common",
                                      "ModelFramework.Common.Builders");
}
