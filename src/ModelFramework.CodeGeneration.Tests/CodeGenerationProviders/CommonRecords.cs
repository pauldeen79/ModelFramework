namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonRecords : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Common";

        public override string DefaultFileName => "Entities.generated.cs";

        protected override object CreateModel()
            => GetImmutableClasses(GetCommonModelTypes(), "ModelFramework.Common");
    }
}
