using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Database\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetDatabaseModelTypes(),
                                          "ModelFramework.Database",
                                          "ModelFramework.Database.Builders");
    }
}
