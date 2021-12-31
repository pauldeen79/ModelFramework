using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record TableField : ITableField
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public TableField(string name,
                          string type,
                          bool isRequired,
                          bool isIdentity,
                          byte? numericPrecision,
                          byte? numericScale,
                          int? stringLength,
                          string stringCollation,
                          bool isStringMaxLength,
                          IEnumerable<ICheckConstraint> checkConstraints,
                          IEnumerable<IMetadata> metadata)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Type cannot be null or whitespace");
            }

            Name = name;
            Type = type;
            IsRequired = isRequired;
            IsIdentity = isIdentity;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            StringLength = stringLength;
            StringCollation = stringCollation;
            IsStringMaxLength = isStringMaxLength;
            CheckConstraints = new ValueCollection<ICheckConstraint>(checkConstraints);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string Type { get; }
        public bool IsIdentity { get; }
        public bool IsRequired { get; }
        public byte? NumericPrecision { get; }
        public byte? NumericScale { get; }
        public int? StringLength { get; }
        public string StringCollation { get; }
        public bool IsStringMaxLength { get; }
        public string Name { get; }
        public ValueCollection<ICheckConstraint> CheckConstraints { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
