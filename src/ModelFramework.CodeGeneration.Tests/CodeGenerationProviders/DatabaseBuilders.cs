namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseBuilders : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Database\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        protected override object CreateModel()
            => GetImmutableBuilderClasses(GetDatabaseModelTypes(),
                                          "ModelFramework.Database",
                                          "ModelFramework.Database.Builders");
    }
}
