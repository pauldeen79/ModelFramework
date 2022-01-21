using ModelFramework.CodeGeneration.Tests.Helpers;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class ObjectsCodeStatements : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetCodeStatementBuilderClasses(typeof(LiteralCodeStatement),
                                              typeof(ICodeStatement),
                                              typeof(ICodeStatementBuilder),
                                              "ModelFramework.Objects.CodeStatements.Builders");
    }
}
