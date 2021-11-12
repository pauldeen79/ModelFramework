using System;
using System.Collections.Generic;
using System.Linq;
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
                          bool isRequired = false,
                          bool isIdentity = false,
                          byte? numericPrecision = null,
                          byte? numericScale = null,
                          int? stringLength = null,
                          string stringCollation = null,
                          bool? isStringMaxLength = null,
                          ITableFieldCheckConstraint checkConstraint = null,
                          IEnumerable<IMetadata> metadata = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Type = type;
            IsRequired = isRequired;
            IsIdentity = isIdentity;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            StringLength = stringLength;
            StringCollation = stringCollation;
            IsStringMaxLength = isStringMaxLength;
            CheckConstraint = checkConstraint;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string /*TableFieldType*/ Type { get; }
        public bool IsIdentity { get; }
        public bool IsRequired { get; }
        public byte? NumericPrecision { get; }
        public byte? NumericScale { get; }
        public int? StringLength { get; }
        public string StringCollation { get; }
        public bool? IsStringMaxLength { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public ITableFieldCheckConstraint CheckConstraint { get; }
    }
}
