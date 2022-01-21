using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Database";

        public override string DefaultFileName => "Entities.generated.cs";

        public override object CreateModel()
            => GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database");
    }
}
