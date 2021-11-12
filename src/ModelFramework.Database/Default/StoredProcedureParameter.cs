using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record StoredProcedureParameter : IStoredProcedureParameter
    {
        public StoredProcedureParameter(string name,
                                        string type,
                                        string defaultValue,
                                        IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Type = type;
            DefaultValue = defaultValue;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string Type { get; }
        public string DefaultValue { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
