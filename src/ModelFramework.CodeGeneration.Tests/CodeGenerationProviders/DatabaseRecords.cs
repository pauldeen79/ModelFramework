namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseRecords : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Database";

        public override string DefaultFileName => "Entities.generated.cs";

        protected override object CreateModel()
            => GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database");
    }
}
