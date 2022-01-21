using ModelFramework.CodeGeneration.Tests.Helpers;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseBuilders : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetImmutableBuilderClasses(GetDatabaseModelTypes(),
                                          "ModelFramework.Database",
                                          "ModelFramework.Database.Builders");
    }
}
