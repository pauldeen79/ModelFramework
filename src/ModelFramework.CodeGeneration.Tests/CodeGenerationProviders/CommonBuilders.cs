namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class CommonBuilders : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Common\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        protected override object CreateModel()
            => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                          "ModelFramework.Common",
                                          "ModelFramework.Common.Builders");
    }
}
