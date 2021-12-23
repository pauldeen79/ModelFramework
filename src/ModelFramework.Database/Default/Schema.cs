using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record Schema : ISchema
    {
        public Schema(string name,
                      IEnumerable<ITable> tables,
                      IEnumerable<IStoredProcedure> storedProcedures,
                      IEnumerable<IView> views,
                      IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Tables = new ValueCollection<ITable>(tables);
            StoredProcedures = new ValueCollection<IStoredProcedure>(storedProcedures);
            Views = new ValueCollection<IView>(views);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ValueCollection<ITable> Tables { get; }
        public ValueCollection<IStoredProcedure> StoredProcedures { get; }
        public ValueCollection<IView> Views { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
