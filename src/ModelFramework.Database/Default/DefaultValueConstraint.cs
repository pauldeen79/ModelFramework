using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record DefaultValueConstraint : IDefaultValueConstraint
    {
        public DefaultValueConstraint(string fieldName,
                                      string defaultValue,
                                      string name,
                                      IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), "FieldName cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                throw new ArgumentOutOfRangeException(nameof(defaultValue), "DefaultValue cannot be null or whitespace");
            }

            FieldName = fieldName;
            DefaultValue = defaultValue;
            Name = name;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string FieldName { get; }
        public string DefaultValue { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
