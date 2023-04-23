﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
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
    public partial class DefaultValueConstraintBuilder
    {
        public System.Text.StringBuilder FieldName
        {
            get
            {
                return _fieldNameDelegate.Value;
            }
            set
            {
                _fieldNameDelegate = new (() => value);
            }
        }

        public System.Text.StringBuilder DefaultValue
        {
            get
            {
                return _defaultValueDelegate.Value;
            }
            set
            {
                _defaultValueDelegate = new (() => value);
            }
        }

        public System.Text.StringBuilder Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IDefaultValueConstraint Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.DefaultValueConstraint(FieldName?.ToString(), DefaultValue?.ToString(), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public DefaultValueConstraintBuilder WithFieldName(System.Text.StringBuilder fieldName)
        {
            FieldName = fieldName;
            return this;
        }

        public DefaultValueConstraintBuilder WithFieldName(System.Func<System.Text.StringBuilder> fieldNameDelegate)
        {
            _fieldNameDelegate = new (fieldNameDelegate);
            return this;
        }

        public DefaultValueConstraintBuilder WithFieldName(string value)
        {
            if (FieldName == null)
                FieldName = new System.Text.StringBuilder();
            FieldName.Clear().Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendToFieldName(string value)
        {
            if (FieldName == null)
                FieldName = new System.Text.StringBuilder();
            FieldName.Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendLineToFieldName(string value)
        {
            if (FieldName == null)
                FieldName = new System.Text.StringBuilder();
            FieldName.AppendLine(value);
            return this;
        }

        public DefaultValueConstraintBuilder WithDefaultValue(System.Text.StringBuilder defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public DefaultValueConstraintBuilder WithDefaultValue(System.Func<System.Text.StringBuilder> defaultValueDelegate)
        {
            _defaultValueDelegate = new (defaultValueDelegate);
            return this;
        }

        public DefaultValueConstraintBuilder WithDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.Clear().Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendToDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendLineToDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.AppendLine(value);
            return this;
        }

        public DefaultValueConstraintBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public DefaultValueConstraintBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public DefaultValueConstraintBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public DefaultValueConstraintBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public DefaultValueConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public DefaultValueConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public DefaultValueConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public DefaultValueConstraintBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _fieldNameDelegate = new (() => new System.Text.StringBuilder());
            _defaultValueDelegate = new (() => new System.Text.StringBuilder());
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public DefaultValueConstraintBuilder(ModelFramework.Database.Contracts.IDefaultValueConstraint source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _fieldNameDelegate = new (() => new System.Text.StringBuilder(source.FieldName));
            _defaultValueDelegate = new (() => new System.Text.StringBuilder(source.DefaultValue));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<System.Text.StringBuilder> _fieldNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _defaultValueDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

