using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Builders
{
    public class TableFieldBuilder
    {
        public string Type { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsRequired { get; set; }
        public byte? NumericPrecision { get; set; }
        public byte? NumericScale { get; set; }
        public int? StringLength { get; set; }
        public string StringCollation { get; set; }
        public bool? IsStringMaxLength { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public ITableFieldCheckConstraint CheckConstraint { get; set; }
        public ITableField Build()
        {
            return new TableField(Name, Type, IsRequired, IsIdentity, NumericPrecision, NumericScale, StringLength, StringCollation, IsStringMaxLength, CheckConstraint, Metadata.Select(x => x.Build()));
        }
        public TableFieldBuilder Clear()
        {
            Type = default;
            IsIdentity = default;
            IsRequired = default;
            NumericPrecision = default;
            NumericScale = default;
            StringLength = default;
            StringCollation = default;
            IsStringMaxLength = default;
            Name = default;
            Metadata.Clear();
            CheckConstraint = default;
            return this;
        }
        public TableFieldBuilder Update(ITableField source)
        {
            Type = default;
            IsIdentity = default;
            IsRequired = default;
            NumericPrecision = default;
            NumericScale = default;
            StringLength = default;
            StringCollation = default;
            IsStringMaxLength = default;
            Name = default;
            Metadata = new List<MetadataBuilder>();
            CheckConstraint = default;
            if (source != null)
            {
                Type = source.Type;
                IsIdentity = source.IsIdentity;
                IsRequired = source.IsRequired;
                NumericPrecision = source.NumericPrecision;
                NumericScale = source.NumericScale;
                StringLength = source.StringLength;
                StringCollation = source.StringCollation;
                IsStringMaxLength = source.IsStringMaxLength;
                Name = source.Name;
                Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                CheckConstraint = source.CheckConstraint;
            }
            return this;
        }
        public TableFieldBuilder WithType(string type)
        {
            Type = type;
            return this;
        }
        public TableFieldBuilder WithIsIdentity(bool isIdentity)
        {
            IsIdentity = isIdentity;
            return this;
        }
        public TableFieldBuilder WithIsRequired(bool isRequired)
        {
            IsRequired = isRequired;
            return this;
        }
        public TableFieldBuilder WithNumericPrecision(byte? numericPrecision)
        {
            NumericPrecision = numericPrecision;
            return this;
        }
        public TableFieldBuilder WithNumericScale(byte? numericScale)
        {
            NumericScale = numericScale;
            return this;
        }
        public TableFieldBuilder WithStringLength(int? stringLength)
        {
            StringLength = stringLength;
            return this;
        }
        public TableFieldBuilder WithStringCollation(string stringCollation)
        {
            StringCollation = stringCollation;
            return this;
        }
        public TableFieldBuilder WithIsStringMaxLength(bool? isStringMaxLength)
        {
            IsStringMaxLength = isStringMaxLength;
            return this;
        }
        public TableFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public TableFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public TableFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public TableFieldBuilder WithCheckConstraint(ITableFieldCheckConstraint checkConstraint)
        {
            CheckConstraint = checkConstraint;
            return this;
        }
        public TableFieldBuilder(ITableField source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Type = source.Type;
                IsIdentity = source.IsIdentity;
                IsRequired = source.IsRequired;
                NumericPrecision = source.NumericPrecision;
                NumericScale = source.NumericScale;
                StringLength = source.StringLength;
                StringCollation = source.StringCollation;
                IsStringMaxLength = source.IsStringMaxLength;
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
                CheckConstraint = source.CheckConstraint;
            }
        }
        public TableFieldBuilder(string name,
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
        {
            Metadata = new List<MetadataBuilder>();
            Type = type;
            IsIdentity = isIdentity;
            IsRequired = isRequired;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            StringLength = stringLength;
            StringCollation = stringCollation;
            IsStringMaxLength = isStringMaxLength;
            Name = name;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            CheckConstraint = checkConstraint;
        }
    }
}
