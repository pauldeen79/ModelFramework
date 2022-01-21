using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                          "ModelFramework.Common",
                                          "ModelFramework.Common.Builders");
    }
}
