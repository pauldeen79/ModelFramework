using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableClasses(GetCommonModelTypes(), "ModelFramework.Common");
    }
}
