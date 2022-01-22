using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsCodeStatements : ModelFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Objects\\CodeStatements\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        protected override object CreateModel()
            => GetCodeStatementBuilderClasses(typeof(LiteralCodeStatement),
                                              typeof(ICodeStatement),
                                              typeof(ICodeStatementBuilder),
                                              "ModelFramework.Objects.CodeStatements.Builders");
    }
}
