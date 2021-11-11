using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Extensions
{
    public static class EnumerableOfStringExtensions
    {
        public static IEnumerable<ICodeStatement> ToLiteralCodeStatements(this IEnumerable<string> instance)
            => instance.Select(s => new LiteralCodeStatement(s));

        public static IEnumerable<ICodeStatementBuilder> ToLiteralCodeStatementBuilders(this IEnumerable<string> instance)
            => instance.Select(s => new LiteralCodeStatementBuilder(s));
    }
}
