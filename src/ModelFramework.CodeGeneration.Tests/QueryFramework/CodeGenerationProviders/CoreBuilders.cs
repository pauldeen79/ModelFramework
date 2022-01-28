using System.Linq;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace ModelFramework.CodeGeneration.Tests.QueryFramework.CodeGenerationProviders
{
    public class CoreBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Core\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableBuilderClasses(GetModels().Where(x => x.Name != "IQueryExpressionFunction").ToArray(),
                                          "QueryFramework.Core",
                                          "QueryFramework.Core.Builders",
                                          "QueryFramework.Abstractions.Builders.I{0}");
    }
}
