using System;
using System.Collections.Generic;
using System.Linq;
using ModelFramework.Objects.Extensions;

namespace ModelFramework.Objects.Builders
{
    public partial class ClassConstructorBuilder
    {
        public ClassConstructorBuilder AddLiteralCodeStatements(params string[] statements)
            => AddCodeStatements(statements.ToLiteralCodeStatementBuilders());

        public ClassConstructorBuilder AddLiteralCodeStatements(IEnumerable<string> statements)
            => AddLiteralCodeStatements(statements.ToArray());

        public ClassConstructorBuilder AddParameter(string name, string typeName)
            => AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));

        public ClassConstructorBuilder AddParameter(string name, Type type)
            => AddParameters(new ParameterBuilder().WithName(name).WithType(type));

        public ClassConstructorBuilder ChainCallToBaseUsingParameters()
            => WithChainCall($"base({string.Join(", ", Parameters.Select(x => x.Name))})");
    }
}
