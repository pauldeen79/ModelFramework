using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class EnumerableOfStringExtensions
    {
        public static IEnumerable<ICodeStatement> ToLiteralCodeStatements(this IEnumerable<string> instance)
            => instance.Select(s => new LiteralCodeStatement(s, Enumerable.Empty<IMetadata>()));

        public static IEnumerable<ICodeStatementBuilder> ToLiteralCodeStatementBuilders(this IEnumerable<string> instance)
            => instance.Select(s => new LiteralCodeStatementBuilder().WithStatement(s));
    }
}
