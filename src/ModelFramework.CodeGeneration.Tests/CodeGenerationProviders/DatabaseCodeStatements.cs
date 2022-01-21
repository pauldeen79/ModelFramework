using ModelFramework.CodeGeneration.Tests.Helpers;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.SqlStatements;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseCodeStatements : CSharpClassBase, ICodeGenerationProvider
    {
        public override object CreateModel()
            => GetCodeStatementBuilderClasses(typeof(LiteralSqlStatement),
                                              typeof(ISqlStatement),
                                              typeof(ISqlStatementBuilder),
                                              "ModelFramework.Database.SqlStatements.Builders");
    }
}
