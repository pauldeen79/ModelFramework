using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Common";

        public override string DefaultFileName => "Entities.generated.cs";

        public override object CreateModel()
            => GetImmutableClasses(GetCommonModelTypes(), "ModelFramework.Common");
    }
}
