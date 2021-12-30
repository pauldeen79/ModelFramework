using System;
using System.Collections.Generic;
using System.Linq;
using ModelFramework.Objects.Extensions;

namespace ModelFramework.Objects.Builders
{
    public partial class ClassMethodBuilder
    {
        public ClassMethodBuilder AddLiteralCodeStatements(params string[] statements)
            => AddCodeStatements(statements.ToLiteralCodeStatementBuilders());

        public ClassMethodBuilder AddLiteralCodeStatements(IEnumerable<string> statements)
            => AddLiteralCodeStatements(statements.ToArray());

        public ClassMethodBuilder AddParameter(string name, Type type)
            => AddParameters(new ParameterBuilder().WithName(name).WithType(type));

        public ClassMethodBuilder AddParameter(string name, string typeName)
            => AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));
    }
}
