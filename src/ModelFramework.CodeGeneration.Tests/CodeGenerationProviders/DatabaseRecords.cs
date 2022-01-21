using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseRecords : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database");
    }
}
