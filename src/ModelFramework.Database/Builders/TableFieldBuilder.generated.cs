﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.14
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Database.Builders
{
#nullable enable
    public partial class TableFieldBuilder
    {
        public string Type
        {
            get;
            set;
        }

        public bool IsIdentity
        {
            get;
            set;
        }

        public bool IsRequired
        {
            get;
            set;
        }

        public System.Nullable<byte> NumericPrecision
        {
            get;
            set;
        }

        public System.Nullable<byte> NumericScale
        {
            get;
            set;
        }

        public System.Nullable<int> StringLength
        {
            get;
            set;
        }

        public string StringCollation
        {
            get;
            set;
        }

        public bool IsStringMaxLength
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder> CheckConstraints
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.ITableField Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.TableField(Type, IsIdentity, IsRequired, NumericPrecision, NumericScale, StringLength, StringCollation, IsStringMaxLength, Name, Metadata.Select(x => x.Build()), CheckConstraints.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TableFieldBuilder WithType(string type)
        {
            Type = type;
            return this;
        }

        public TableFieldBuilder WithIsIdentity(bool isIdentity = true)
        {
            IsIdentity = isIdentity;
            return this;
        }

        public TableFieldBuilder WithIsRequired(bool isRequired = true)
        {
            IsRequired = isRequired;
            return this;
        }

        public TableFieldBuilder WithNumericPrecision(System.Nullable<byte> numericPrecision)
        {
            NumericPrecision = numericPrecision;
            return this;
        }

        public TableFieldBuilder WithNumericScale(System.Nullable<byte> numericScale)
        {
            NumericScale = numericScale;
            return this;
        }

        public TableFieldBuilder WithStringLength(System.Nullable<int> stringLength)
        {
            StringLength = stringLength;
            return this;
        }

        public TableFieldBuilder WithStringCollation(string stringCollation)
        {
            StringCollation = stringCollation;
            return this;
        }

        public TableFieldBuilder WithIsStringMaxLength(bool isStringMaxLength = true)
        {
            IsStringMaxLength = isStringMaxLength;
            return this;
        }

        public TableFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public TableFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public TableFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public TableFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public TableFieldBuilder AddCheckConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.CheckConstraintBuilder> checkConstraints)
        {
            return AddCheckConstraints(checkConstraints.ToArray());
        }

        public TableFieldBuilder AddCheckConstraints(params ModelFramework.Database.Builders.CheckConstraintBuilder[] checkConstraints)
        {
            CheckConstraints.AddRange(checkConstraints);
            return this;
        }

        public TableFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            CheckConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder>();
            Type = string.Empty;
            StringCollation = string.Empty;
            Name = string.Empty;
        }

        public TableFieldBuilder(ModelFramework.Database.Contracts.ITableField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            CheckConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder>();
            Type = source.Type;
            IsIdentity = source.IsIdentity;
            IsRequired = source.IsRequired;
            NumericPrecision = source.NumericPrecision;
            NumericScale = source.NumericScale;
            StringLength = source.StringLength;
            StringCollation = source.StringCollation;
            IsStringMaxLength = source.IsStringMaxLength;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            CheckConstraints.AddRange(source.CheckConstraints.Select(x => new ModelFramework.Database.Builders.CheckConstraintBuilder(x)));
        }
    }
#nullable restore
}

