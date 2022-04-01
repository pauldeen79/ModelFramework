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

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class ParameterBuilder
    {
        public bool IsParamArray
        {
            get
            {
                return _isParamArrayDelegate.Value;
            }
            set
            {
                _isParamArrayDelegate = new (() => value);
            }
        }

        public string TypeName
        {
            get
            {
                return _typeNameDelegate.Value;
            }
            set
            {
                _typeNameDelegate = new (() => value);
            }
        }

        public bool IsNullable
        {
            get
            {
                return _isNullableDelegate.Value;
            }
            set
            {
                _isNullableDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
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

        public object? DefaultValue
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

        public ParameterBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ParameterBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ParameterBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ParameterBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ParameterBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Objects.Contracts.IParameter Build()
        {
            return new ModelFramework.Objects.Parameter(IsParamArray, TypeName, IsNullable, Attributes.Select(x => x.Build()), Metadata.Select(x => x.Build()), Name, DefaultValue);
        }

        public ParameterBuilder WithDefaultValue(System.Func<object?> defaultValueDelegate)
        {
            _defaultValueDelegate = new (defaultValueDelegate);
            return this;
        }

        public ParameterBuilder WithDefaultValue(object? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public ParameterBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ParameterBuilder WithIsNullable(System.Func<bool> isNullableDelegate)
        {
            _isNullableDelegate = new (isNullableDelegate);
            return this;
        }

        public ParameterBuilder WithIsParamArray(bool isParamArray = true)
        {
            IsParamArray = isParamArray;
            return this;
        }

        public ParameterBuilder WithIsParamArray(System.Func<bool> isParamArrayDelegate)
        {
            _isParamArrayDelegate = new (isParamArrayDelegate);
            return this;
        }

        public ParameterBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ParameterBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName;
            return this;
        }

        public ParameterBuilder WithTypeName(System.Func<string> typeNameDelegate)
        {
            _typeNameDelegate = new (typeNameDelegate);
            return this;
        }

        public ParameterBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ParameterBuilder()
        {
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isParamArrayDelegate = new (() => default);
            _typeNameDelegate = new (() => string.Empty);
            _isNullableDelegate = new (() => default);
            _nameDelegate = new (() => string.Empty);
            _defaultValueDelegate = new (() => default);
        }

        public ParameterBuilder(ModelFramework.Objects.Contracts.IParameter source)
        {
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isParamArrayDelegate = new (() => source.IsParamArray);
            _typeNameDelegate = new (() => source.TypeName);
            _isNullableDelegate = new (() => source.IsNullable);
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _nameDelegate = new (() => source.Name);
            _defaultValueDelegate = new (() => source.DefaultValue);
        }

        private System.Lazy<bool> _isParamArrayDelegate;

        private System.Lazy<string> _typeNameDelegate;

        private System.Lazy<bool> _isNullableDelegate;

        private System.Lazy<string> _nameDelegate;

        private System.Lazy<object?> _defaultValueDelegate;
    }
#nullable restore
}

