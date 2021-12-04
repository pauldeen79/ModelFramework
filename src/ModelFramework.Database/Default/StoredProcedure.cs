using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record StoredProcedure : IStoredProcedure
    {
        public StoredProcedure(string name,
                               IEnumerable<IStoredProcedureParameter> parameters = null,
                               IEnumerable<ISqlStatement> statements = null,
                               IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Parameters = new ValueCollection<IStoredProcedureParameter>(parameters ?? Enumerable.Empty<IStoredProcedureParameter>());
            Statements = new ValueCollection<ISqlStatement>(statements ?? Enumerable.Empty<ISqlStatement>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<IStoredProcedureParameter> Parameters { get; }
        public string Name { get; }
        public ValueCollection<ISqlStatement> Statements { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
