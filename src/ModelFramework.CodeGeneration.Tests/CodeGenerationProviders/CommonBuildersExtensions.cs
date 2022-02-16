namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonBuildersExtensions : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Common\\Builders";

    public override string DefaultFileName => "Extensions.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses(GetCommonModelTypes(),
                                               "ModelFramework.Common",
                                               "ModelFramework.Common.Extensions",
                                               "ModelFramework.Common.Contracts.Builders");
}
