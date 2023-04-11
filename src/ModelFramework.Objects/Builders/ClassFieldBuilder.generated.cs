﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.4
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
    public partial class ClassFieldBuilder
    {
        public bool ReadOnly
        {
            get
            {
                return _readOnlyDelegate.Value;
            }
            set
            {
                _readOnlyDelegate = new (() => value);
            }
        }

        public bool Constant
        {
            get
            {
                return _constantDelegate.Value;
            }
            set
            {
                _constantDelegate = new (() => value);
            }
        }

        public bool Event
        {
            get
            {
                return _eventDelegate.Value;
            }
            set
            {
                _eventDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public bool Static
        {
            get
            {
                return _staticDelegate.Value;
            }
            set
            {
                _staticDelegate = new (() => value);
            }
        }

        public bool Virtual
        {
            get
            {
                return _virtualDelegate.Value;
            }
            set
            {
                _virtualDelegate = new (() => value);
            }
        }

        public bool Abstract
        {
            get
            {
                return _abstractDelegate.Value;
            }
            set
            {
                _abstractDelegate = new (() => value);
            }
        }

        public bool Protected
        {
            get
            {
                return _protectedDelegate.Value;
            }
            set
            {
                _protectedDelegate = new (() => value);
            }
        }

        public bool Override
        {
            get
            {
                return _overrideDelegate.Value;
            }
            set
            {
                _overrideDelegate = new (() => value);
            }
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get
            {
                return _visibilityDelegate.Value;
            }
            set
            {
                _visibilityDelegate = new (() => value);
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

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public System.Text.StringBuilder TypeName
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

        public bool IsValueType
        {
            get
            {
                return _isValueTypeDelegate.Value;
            }
            set
            {
                _isValueTypeDelegate = new (() => value);
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

        public System.Text.StringBuilder ParentTypeFullName
        {
            get
            {
                return _parentTypeFullNameDelegate.Value;
            }
            set
            {
                _parentTypeFullNameDelegate = new (() => value);
            }
        }

        public ModelFramework.Objects.Contracts.IClassField Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.ClassField(ReadOnly, Constant, Event, Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name?.ToString(), Attributes.Select(x => x.Build()), TypeName?.ToString(), IsNullable, IsValueType, DefaultValue, ParentTypeFullName?.ToString());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassFieldBuilder WithReadOnly(bool readOnly = true)
        {
            ReadOnly = readOnly;
            return this;
        }

        public ClassFieldBuilder WithReadOnly(System.Func<bool> readOnlyDelegate)
        {
            _readOnlyDelegate = new (readOnlyDelegate);
            return this;
        }

        public ClassFieldBuilder WithConstant(bool constant = true)
        {
            Constant = constant;
            return this;
        }

        public ClassFieldBuilder WithConstant(System.Func<bool> constantDelegate)
        {
            _constantDelegate = new (constantDelegate);
            return this;
        }

        public ClassFieldBuilder WithEvent(bool @event = true)
        {
            Event = @event;
            return this;
        }

        public ClassFieldBuilder WithEvent(System.Func<bool> eventDelegate)
        {
            _eventDelegate = new (@eventDelegate);
            return this;
        }

        public ClassFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassFieldBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassFieldBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassFieldBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassFieldBuilder WithVirtual(System.Func<bool> virtualDelegate)
        {
            _virtualDelegate = new (@virtualDelegate);
            return this;
        }

        public ClassFieldBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassFieldBuilder WithAbstract(System.Func<bool> abstractDelegate)
        {
            _abstractDelegate = new (@abstractDelegate);
            return this;
        }

        public ClassFieldBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassFieldBuilder WithProtected(System.Func<bool> protectedDelegate)
        {
            _protectedDelegate = new (@protectedDelegate);
            return this;
        }

        public ClassFieldBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassFieldBuilder WithOverride(System.Func<bool> overrideDelegate)
        {
            _overrideDelegate = new (@overrideDelegate);
            return this;
        }

        public ClassFieldBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassFieldBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public ClassFieldBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public ClassFieldBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ClassFieldBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public ClassFieldBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public ClassFieldBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public ClassFieldBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassFieldBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassFieldBuilder WithTypeName(System.Text.StringBuilder typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ClassFieldBuilder WithTypeName(System.Func<System.Text.StringBuilder> typeNameDelegate)
        {
            _typeNameDelegate = new (typeNameDelegate);
            return this;
        }

        public ClassFieldBuilder WithTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.Clear().Append(value);
            return this;
        }

        public ClassFieldBuilder AppendToTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.Append(value);
            return this;
        }

        public ClassFieldBuilder AppendLineToTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.AppendLine(value);
            return this;
        }

        public ClassFieldBuilder WithType(System.Type type)
        {
            TypeName.Clear().Append(type.AssemblyQualifiedName.FixTypeName()); IsValueType = type.IsValueType || type.IsEnum;
            return this;
        }

        public ClassFieldBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassFieldBuilder WithIsNullable(System.Func<bool> isNullableDelegate)
        {
            _isNullableDelegate = new (isNullableDelegate);
            return this;
        }

        public ClassFieldBuilder WithIsValueType(bool isValueType = true)
        {
            IsValueType = isValueType;
            return this;
        }

        public ClassFieldBuilder WithIsValueType(System.Func<bool> isValueTypeDelegate)
        {
            _isValueTypeDelegate = new (isValueTypeDelegate);
            return this;
        }

        public ClassFieldBuilder WithDefaultValue(object? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public ClassFieldBuilder WithDefaultValue(System.Func<object?> defaultValueDelegate)
        {
            _defaultValueDelegate = new (defaultValueDelegate);
            return this;
        }

        public ClassFieldBuilder WithParentTypeFullName(System.Text.StringBuilder parentTypeFullName)
        {
            ParentTypeFullName = parentTypeFullName;
            return this;
        }

        public ClassFieldBuilder WithParentTypeFullName(System.Func<System.Text.StringBuilder> parentTypeFullNameDelegate)
        {
            _parentTypeFullNameDelegate = new (parentTypeFullNameDelegate);
            return this;
        }

        public ClassFieldBuilder WithParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.Clear().Append(value);
            return this;
        }

        public ClassFieldBuilder AppendToParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.Append(value);
            return this;
        }

        public ClassFieldBuilder AppendLineToParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.AppendLine(value);
            return this;
        }

        public ClassFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _readOnlyDelegate = new (() => default);
            _constantDelegate = new (() => default);
            _eventDelegate = new (() => default);
            _staticDelegate = new (() => default);
            _virtualDelegate = new (() => default);
            _abstractDelegate = new (() => default);
            _protectedDelegate = new (() => default);
            _overrideDelegate = new (() => default);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Private);
            _nameDelegate = new (() => new System.Text.StringBuilder());
            _typeNameDelegate = new (() => new System.Text.StringBuilder());
            _isNullableDelegate = new (() => default);
            _isValueTypeDelegate = new (() => default);
            _defaultValueDelegate = new (() => default);
            _parentTypeFullNameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassFieldBuilder(ModelFramework.Objects.Contracts.IClassField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            _readOnlyDelegate = new (() => source.ReadOnly);
            _constantDelegate = new (() => source.Constant);
            _eventDelegate = new (() => source.Event);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _staticDelegate = new (() => source.Static);
            _virtualDelegate = new (() => source.Virtual);
            _abstractDelegate = new (() => source.Abstract);
            _protectedDelegate = new (() => source.Protected);
            _overrideDelegate = new (() => source.Override);
            _visibilityDelegate = new (() => source.Visibility);
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            _typeNameDelegate = new (() => new System.Text.StringBuilder(source.TypeName));
            _isNullableDelegate = new (() => source.IsNullable);
            _isValueTypeDelegate = new (() => source.IsValueType);
            _defaultValueDelegate = new (() => source.DefaultValue);
            _parentTypeFullNameDelegate = new (() => new System.Text.StringBuilder(source.ParentTypeFullName));
        }

        protected System.Lazy<bool> _readOnlyDelegate;

        protected System.Lazy<bool> _constantDelegate;

        protected System.Lazy<bool> _eventDelegate;

        protected System.Lazy<bool> _staticDelegate;

        protected System.Lazy<bool> _virtualDelegate;

        protected System.Lazy<bool> _abstractDelegate;

        protected System.Lazy<bool> _protectedDelegate;

        protected System.Lazy<bool> _overrideDelegate;

        protected System.Lazy<ModelFramework.Objects.Contracts.Visibility> _visibilityDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _typeNameDelegate;

        protected System.Lazy<bool> _isNullableDelegate;

        protected System.Lazy<bool> _isValueTypeDelegate;

        protected System.Lazy<object?> _defaultValueDelegate;

        protected System.Lazy<System.Text.StringBuilder> _parentTypeFullNameDelegate;
    }
#nullable restore
}

