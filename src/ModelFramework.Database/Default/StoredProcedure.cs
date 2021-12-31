using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record StoredProcedure : IStoredProcedure
    {
        public StoredProcedure(string name,
                               IEnumerable<IStoredProcedureParameter> parameters,
                               IEnumerable<ISqlStatement> statements,
                               IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Parameters = new ValueCollection<IStoredProcedureParameter>(parameters);
            Statements = new ValueCollection<ISqlStatement>(statements);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ValueCollection<IStoredProcedureParameter> Parameters { get; }
        public string Name { get; }
        public ValueCollection<ISqlStatement> Statements { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
