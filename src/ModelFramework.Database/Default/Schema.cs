using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record Schema : ISchema
    {
        public Schema(string name,
                      IEnumerable<ITable>? tables = null,
                      IEnumerable<IStoredProcedure>? storedProcedures = null,
                      IEnumerable<IView>? views = null,
                      IEnumerable<IMetadata>? metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Tables = new ValueCollection<ITable>(tables ?? Enumerable.Empty<ITable>());
            StoredProcedures = new ValueCollection<IStoredProcedure>(storedProcedures ?? Enumerable.Empty<IStoredProcedure>());
            Views = new ValueCollection<IView>(views ?? Enumerable.Empty<IView>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<ITable> Tables { get; }
        public ValueCollection<IStoredProcedure> StoredProcedures { get; }
        public ValueCollection<IView> Views { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
