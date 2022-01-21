using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Common\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                          "ModelFramework.Common",
                                          "ModelFramework.Common.Builders");
    }
}
