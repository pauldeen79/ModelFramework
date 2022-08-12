﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.8
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

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
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

        public string ExplicitInterfaceName
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

        public string ParentTypeFullName
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

        public ClassPropertyBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassPropertyBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassPropertyBuilder AddGetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] getterCodeStatements)
        {
            GetterCodeStatements.AddRange(getterCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddGetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> getterCodeStatements)
        {
            return AddGetterCodeStatements(getterCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] initializerCodeStatements)
        {
            InitializerCodeStatements.AddRange(initializerCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> initializerCodeStatements)
        {
            return AddInitializerCodeStatements(initializerCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassPropertyBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassPropertyBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassPropertyBuilder AddSetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] setterCodeStatements)
        {
            SetterCodeStatements.AddRange(setterCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddSetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> setterCodeStatements)
        {
            return AddSetterCodeStatements(setterCodeStatements.ToArray());
        }

        public ModelFramework.Objects.Contracts.IClassProperty Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.ClassProperty(HasGetter, HasSetter, HasInitializer, GetterVisibility, SetterVisibility, InitializerVisibility, GetterCodeStatements.Select(x => x.Build()), SetterCodeStatements.Select(x => x.Build()), InitializerCodeStatements.Select(x => x.Build()), Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.Build()), TypeName, IsNullable, ExplicitInterfaceName, ParentTypeFullName);
            #pragma warning restore CS8604 // Possible null reference argument.
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

        public ClassPropertyBuilder WithExplicitInterfaceName(System.Func<string> explicitInterfaceNameDelegate)
        {
            _explicitInterfaceNameDelegate = new (explicitInterfaceNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }

        public ClassPropertyBuilder WithGetterVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> getterVisibilityDelegate)
        {
            _getterVisibilityDelegate = new (getterVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithGetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> getterVisibility)
        {
            GetterVisibility = getterVisibility;
            return this;
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

        public ClassPropertyBuilder WithInitializerVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> initializerVisibilityDelegate)
        {
            _initializerVisibilityDelegate = new (initializerVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithInitializerVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> initializerVisibility)
        {
            InitializerVisibility = initializerVisibility;
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

        public ClassPropertyBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithName(string name)
        {
            Name = name;
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

        public ClassPropertyBuilder WithParentTypeFullName(System.Func<string> parentTypeFullNameDelegate)
        {
            _parentTypeFullNameDelegate = new (parentTypeFullNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithParentTypeFullName(string parentTypeFullName)
        {
            ParentTypeFullName = parentTypeFullName;
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

        public ClassPropertyBuilder WithSetterVisibility(System.Func<System.Nullable<ModelFramework.Objects.Contracts.Visibility>> setterVisibilityDelegate)
        {
            _setterVisibilityDelegate = new (setterVisibilityDelegate);
            return this;
        }

        public ClassPropertyBuilder WithSetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> setterVisibility)
        {
            SetterVisibility = setterVisibility;
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

        public ClassPropertyBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName;
            return this;
        }

        public ClassPropertyBuilder WithTypeName(System.Func<string> typeNameDelegate)
        {
            _typeNameDelegate = new (typeNameDelegate);
            return this;
        }

        public ClassPropertyBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
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
            _nameDelegate = new (() => string.Empty);
            _typeNameDelegate = new (() => string.Empty);
            _isNullableDelegate = new (() => default);
            _explicitInterfaceNameDelegate = new (() => string.Empty);
            _parentTypeFullNameDelegate = new (() => string.Empty);
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
            _nameDelegate = new (() => source.Name);
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            _typeNameDelegate = new (() => source.TypeName);
            _isNullableDelegate = new (() => source.IsNullable);
            _explicitInterfaceNameDelegate = new (() => source.ExplicitInterfaceName);
            _parentTypeFullNameDelegate = new (() => source.ParentTypeFullName);
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

        protected System.Lazy<string> _nameDelegate;

        protected System.Lazy<string> _typeNameDelegate;

        protected System.Lazy<bool> _isNullableDelegate;

        protected System.Lazy<string> _explicitInterfaceNameDelegate;

        protected System.Lazy<string> _parentTypeFullNameDelegate;
    }
#nullable restore
}

