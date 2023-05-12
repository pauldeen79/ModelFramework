namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonModels : ModelFrameworkModelClassBase
{
    public override string Path => "ModelFramework.Common/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                      "ModelFramework.Common",
                                      "ModelFramework.Common.Models");
}
