﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
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
        public string Type
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

        public string DefaultValue
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

        public string Name
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

        public StoredProcedureParameterBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public StoredProcedureParameterBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public StoredProcedureParameterBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IStoredProcedureParameter Build()
        {
            return new ModelFramework.Database.StoredProcedureParameter(Type, DefaultValue, Name, Metadata.Select(x => x.Build()));
        }

        public StoredProcedureParameterBuilder WithDefaultValue(System.Func<string> defaultValueDelegate)
        {
            _defaultValueDelegate = new (defaultValueDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public StoredProcedureParameterBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public StoredProcedureParameterBuilder WithType(System.Func<string> typeDelegate)
        {
            _typeDelegate = new (typeDelegate);
            return this;
        }

        public StoredProcedureParameterBuilder WithType(string type)
        {
            Type = type;
            return this;
        }

        public StoredProcedureParameterBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _typeDelegate = new (() => string.Empty);
            _defaultValueDelegate = new (() => string.Empty);
            _nameDelegate = new (() => string.Empty);
        }

        public StoredProcedureParameterBuilder(ModelFramework.Database.Contracts.IStoredProcedureParameter source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _typeDelegate = new (() => source.Type);
            _defaultValueDelegate = new (() => source.DefaultValue);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<string> _typeDelegate;

        private System.Lazy<string> _defaultValueDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

