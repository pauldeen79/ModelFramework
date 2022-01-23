using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonBuilders : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "ModelFramework.Common\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                          "ModelFramework.Common",
                                          "ModelFramework.Common.Builders");
    }
}
