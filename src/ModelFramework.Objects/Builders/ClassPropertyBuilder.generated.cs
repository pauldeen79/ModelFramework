﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.9
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
    public partial class ClassPropertyBuilder
    {
        public bool HasGetter
        {
            get
            {
                return _hasGetterDelegate.Value;
            }
            set
            {
                _hasGetterDelegate = new (() => value);
            }
        }

        public bool HasSetter
        {
            get
            {
                return _hasSetterDelegate.Value;
            }
            set
            {
                _hasSetterDelegate = new (() => value);
            }
        }

        public bool HasInitializer
        {
            get
            {
                return _hasInitializerDelegate.Value;
            }
            set
            {
                _hasInitializerDelegate = new (() => value);
            }
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> GetterVisibility
        {
            get
            {
                return _getterVisibilityDelegate.Value;
            }
            set
            {
                _getterVisibilityDelegate = new (() => value);
            }
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> SetterVisibility
        {
            get
            {
                return _setterVisibilityDelegate.Value;
            }
            set
            {
                _setterVisibilityDelegate = new (() => value);
            }
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> InitializerVisibility
        {
            get
            {
                return _initializerVisibilityDelegate.Value;
            }
            set
            {
                _initializerVisibilityDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> GetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> SetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> InitializerCodeStatements
        {
            get;
            set;
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

        public System.Text.StringBuilder ExplicitInterfaceName
        {
            get
            {
                return _explicitInterfaceNameDelegate.Value;
            }
            set
            {
                _explicitInterfaceNameDelegate = new (() => value);
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

        public ModelFramework.Objects.Contracts.IClassProperty Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.ClassProperty(HasGetter, HasSetter, HasInitializer, GetterVisibility, SetterVisibility, InitializerVisibility, GetterCodeStatements.Select(x => x.Build()), SetterCodeStatements.Select(x => x.Build()), InitializerCodeStatements.Select(x => x.Build()), Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name?.ToString(), Attributes.Select(x => x.Build()), TypeName?.ToString(), IsNullable, IsValueType, ExplicitInterfaceName?.ToString(), ParentTypeFullName?.ToString());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassPropertyBuilder WithHasGetter(bool hasGetter = true)
        {
            HasGetter = hasGetter;
            return this;
        }

        public ClassPropertyBuilder WithHasGetter(System.Func<bool> hasGetterDelegate)
        {
            _hasGetterDelegate = new (hasGetterDelegate);
            return this;
        }

        public ClassPropertyBuilder WithHasSetter(bool hasSetter = true)
        {
            HasSetter = hasSetter;
            if (hasSetter)
            {
                HasInitializer = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithHasSetter(System.Func<bool> hasSetterDelegate)
        {
            HasSetter = hasSetterDelegate.Invoke();
            if (hasSetterDelegate.Invoke())
            {
                HasInitializer = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithHasInitializer(bool hasInitializer = true)
        {
            HasInitializer = hasInitializer;
            if (hasInitializer)
            {
                HasSetter = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithHasInitializer(System.Func<bool> hasInitializerDelegate)
        {
            HasInitializer = hasInitializerDelegate.Invoke();
            if (hasInitializerDelegate.Invoke())
            {
                HasSetter = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithGetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> getterVisibility)
        {
            GetterVisibility = getterVisibility;
            return this;
        }

        public ClassPropertyBuilder WithGetterVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> getterVisibilityDelegate)
        {
            _getterVisibilityDelegate = new (getterVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithSetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> setterVisibility)
        {
            SetterVisibility = setterVisibility;
            return this;
        }

        public ClassPropertyBuilder WithSetterVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> setterVisibilityDelegate)
        {
            _setterVisibilityDelegate = new (setterVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithInitializerVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> initializerVisibility)
        {
            InitializerVisibility = initializerVisibility;
            return this;
        }

        public ClassPropertyBuilder WithInitializerVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> initializerVisibilityDelegate)
        {
            _initializerVisibilityDelegate = new (initializerVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder AddGetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> getterCodeStatements)
        {
            return AddGetterCodeStatements(getterCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddGetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] getterCodeStatements)
        {
            GetterCodeStatements.AddRange(getterCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddSetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> setterCodeStatements)
        {
            return AddSetterCodeStatements(setterCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddSetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] setterCodeStatements)
        {
            SetterCodeStatements.AddRange(setterCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> initializerCodeStatements)
        {
            return AddInitializerCodeStatements(initializerCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] initializerCodeStatements)
        {
            InitializerCodeStatements.AddRange(initializerCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassPropertyBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassPropertyBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassPropertyBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassPropertyBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassPropertyBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassPropertyBuilder WithVirtual(System.Func<bool> virtualDelegate)
        {
            _virtualDelegate = new (@virtualDelegate);
            return this;
        }

        public ClassPropertyBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassPropertyBuilder WithAbstract(System.Func<bool> abstractDelegate)
        {
            _abstractDelegate = new (@abstractDelegate);
            return this;
        }

        public ClassPropertyBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassPropertyBuilder WithProtected(System.Func<bool> protectedDelegate)
        {
            _protectedDelegate = new (@protectedDelegate);
            return this;
        }

        public ClassPropertyBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassPropertyBuilder WithOverride(System.Func<bool> overrideDelegate)
        {
            _overrideDelegate = new (@overrideDelegate);
            return this;
        }

        public ClassPropertyBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassPropertyBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public ClassPropertyBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public ClassPropertyBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassPropertyBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassPropertyBuilder WithTypeName(System.Text.StringBuilder typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ClassPropertyBuilder WithTypeName(System.Func<System.Text.StringBuilder> typeNameDelegate)
        {
            _typeNameDelegate = new (typeNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.Clear().Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendToTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendLineToTypeName(string value)
        {
            if (TypeName == null)
                TypeName = new System.Text.StringBuilder();
            TypeName.AppendLine(value);
            return this;
        }

        public ClassPropertyBuilder WithType(System.Type type)
        {
            TypeName.Clear().Append(type.AssemblyQualifiedName.FixTypeName()); IsValueType = type.IsValueType || type.IsEnum;
            return this;
        }

        public ClassPropertyBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassPropertyBuilder WithIsNullable(System.Func<bool> isNullableDelegate)
        {
            _isNullableDelegate = new (isNullableDelegate);
            return this;
        }

        public ClassPropertyBuilder WithIsValueType(bool isValueType = true)
        {
            IsValueType = isValueType;
            return this;
        }

        public ClassPropertyBuilder WithIsValueType(System.Func<bool> isValueTypeDelegate)
        {
            _isValueTypeDelegate = new (isValueTypeDelegate);
            return this;
        }

        public ClassPropertyBuilder WithExplicitInterfaceName(System.Text.StringBuilder explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }

        public ClassPropertyBuilder WithExplicitInterfaceName(System.Func<System.Text.StringBuilder> explicitInterfaceNameDelegate)
        {
            _explicitInterfaceNameDelegate = new (explicitInterfaceNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithExplicitInterfaceName(string value)
        {
            if (ExplicitInterfaceName == null)
                ExplicitInterfaceName = new System.Text.StringBuilder();
            ExplicitInterfaceName.Clear().Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendToExplicitInterfaceName(string value)
        {
            if (ExplicitInterfaceName == null)
                ExplicitInterfaceName = new System.Text.StringBuilder();
            ExplicitInterfaceName.Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendLineToExplicitInterfaceName(string value)
        {
            if (ExplicitInterfaceName == null)
                ExplicitInterfaceName = new System.Text.StringBuilder();
            ExplicitInterfaceName.AppendLine(value);
            return this;
        }

        public ClassPropertyBuilder WithParentTypeFullName(System.Text.StringBuilder parentTypeFullName)
        {
            ParentTypeFullName = parentTypeFullName;
            return this;
        }

        public ClassPropertyBuilder WithParentTypeFullName(System.Func<System.Text.StringBuilder> parentTypeFullNameDelegate)
        {
            _parentTypeFullNameDelegate = new (parentTypeFullNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.Clear().Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendToParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.Append(value);
            return this;
        }

        public ClassPropertyBuilder AppendLineToParentTypeFullName(string value)
        {
            if (ParentTypeFullName == null)
                ParentTypeFullName = new System.Text.StringBuilder();
            ParentTypeFullName.AppendLine(value);
            return this;
        }

        public ClassPropertyBuilder()
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _hasGetterDelegate = new (() => true);
            _hasSetterDelegate = new (() => true);
            _hasInitializerDelegate = new (() => default);
            _getterVisibilityDelegate = new (() => default);
            _setterVisibilityDelegate = new (() => default);
            _initializerVisibilityDelegate = new (() => default);
            _staticDelegate = new (() => default);
            _virtualDelegate = new (() => default);
            _abstractDelegate = new (() => default);
            _protectedDelegate = new (() => default);
            _overrideDelegate = new (() => default);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Public);
            _nameDelegate = new (() => new System.Text.StringBuilder());
            _typeNameDelegate = new (() => new System.Text.StringBuilder());
            _isNullableDelegate = new (() => default);
            _isValueTypeDelegate = new (() => default);
            _explicitInterfaceNameDelegate = new (() => new System.Text.StringBuilder());
            _parentTypeFullNameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassPropertyBuilder(ModelFramework.Objects.Contracts.IClassProperty source)
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            _hasGetterDelegate = new (() => source.HasGetter);
            _hasSetterDelegate = new (() => source.HasSetter);
            _hasInitializerDelegate = new (() => source.HasInitializer);
            _getterVisibilityDelegate = new (() => source.GetterVisibility);
            _setterVisibilityDelegate = new (() => source.SetterVisibility);
            _initializerVisibilityDelegate = new (() => source.InitializerVisibility);
            GetterCodeStatements.AddRange(source.GetterCodeStatements.Select(x => x.CreateBuilder()));
            SetterCodeStatements.AddRange(source.SetterCodeStatements.Select(x => x.CreateBuilder()));
            InitializerCodeStatements.AddRange(source.InitializerCodeStatements.Select(x => x.CreateBuilder()));
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
            _explicitInterfaceNameDelegate = new (() => new System.Text.StringBuilder(source.ExplicitInterfaceName));
            _parentTypeFullNameDelegate = new (() => new System.Text.StringBuilder(source.ParentTypeFullName));
        }

        protected System.Lazy<bool> _hasGetterDelegate;

        protected System.Lazy<bool> _hasSetterDelegate;

        protected System.Lazy<bool> _hasInitializerDelegate;

        protected System.Lazy<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> _getterVisibilityDelegate;

        protected System.Lazy<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> _setterVisibilityDelegate;

        protected System.Lazy<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> _initializerVisibilityDelegate;

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

        protected System.Lazy<System.Text.StringBuilder> _explicitInterfaceNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _parentTypeFullNameDelegate;
    }
#nullable restore
}

