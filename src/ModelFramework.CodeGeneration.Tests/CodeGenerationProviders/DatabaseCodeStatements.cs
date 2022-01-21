using ModelFramework.CodeGeneration.Tests.Helpers;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.SqlStatements;

namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders
{
    public class DatabaseCodeStatements : CSharpClassBase, ICodeGenerationProvider
    {
        public override string Prefix => "ModelFramework.Database\\SqlStatements\\Builders";

        public override string DefaultFileName => "Builders.Generated.cs";

        public override object CreateModel()
            => GetCodeStatementBuilderClasses(typeof(LiteralSqlStatement),
                                              typeof(ISqlStatement),
                                              typeof(ISqlStatementBuilder),
                                              "ModelFramework.Database.SqlStatements.Builders");
    }
}
