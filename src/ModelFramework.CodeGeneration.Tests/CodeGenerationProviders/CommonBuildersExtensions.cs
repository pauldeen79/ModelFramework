namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonBuildersExtensions : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ModelFramework.Common\\Builders";

    public override string DefaultFileName => "Extensions.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses(GetCommonModelTypes(),
                                               "ModelFramework.Common",
                                               "ModelFramework.Common.Builders");
}
