﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.10
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
    public partial class StoredProcedureParameterBuilder
    {
        public System.Text.StringBuilder Type
        {
            get
            {
                return _typeDelegate.Value;
            }
            set
            {
                _typeDelegate = new (() => value);
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

        public ModelFramework.Database.Contracts.IStoredProcedureParameter Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.StoredProcedureParameter(Type?.ToString(), DefaultValue?.ToString(), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public StoredProcedureParameterBuilder WithType(System.Text.StringBuilder type)
        {
            Type = type;
            return this;
        }

        public StoredProcedureParameterBuilder WithType(System.Func<System.Text.StringBuilder> typeDelegate)
        {
            _typeDelegate = new (typeDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithType(string value)
        {
            if (Type == null)
                Type = new System.Text.StringBuilder();
            Type.Clear().Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendToType(string value)
        {
            if (Type == null)
                Type = new System.Text.StringBuilder();
            Type.Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendLineToType(string value)
        {
            if (Type == null)
                Type = new System.Text.StringBuilder();
            Type.AppendLine(value);
            return this;
        }

        public StoredProcedureParameterBuilder WithDefaultValue(System.Text.StringBuilder defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public StoredProcedureParameterBuilder WithDefaultValue(System.Func<System.Text.StringBuilder> defaultValueDelegate)
        {
            _defaultValueDelegate = new (defaultValueDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.Clear().Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendToDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendLineToDefaultValue(string value)
        {
            if (DefaultValue == null)
                DefaultValue = new System.Text.StringBuilder();
            DefaultValue.AppendLine(value);
            return this;
        }

        public StoredProcedureParameterBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public StoredProcedureParameterBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public StoredProcedureParameterBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public StoredProcedureParameterBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public StoredProcedureParameterBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public StoredProcedureParameterBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public StoredProcedureParameterBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _typeDelegate = new (() => new System.Text.StringBuilder());
            _defaultValueDelegate = new (() => new System.Text.StringBuilder());
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public StoredProcedureParameterBuilder(ModelFramework.Database.Contracts.IStoredProcedureParameter source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _typeDelegate = new (() => new System.Text.StringBuilder(source.Type));
            _defaultValueDelegate = new (() => new System.Text.StringBuilder(source.DefaultValue));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<System.Text.StringBuilder> _typeDelegate;

        protected System.Lazy<System.Text.StringBuilder> _defaultValueDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

