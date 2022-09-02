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
    public partial class ClassMethodBuilder
    {
        public bool Partial
        {
            get
            {
                return _partialDelegate.Value;
            }
            set
            {
                _partialDelegate = new (() => value);
            }
        }

        public bool ExtensionMethod
        {
            get
            {
                return _extensionMethodDelegate.Value;
            }
            set
            {
                _extensionMethodDelegate = new (() => value);
            }
        }

        public bool Operator
        {
            get
            {
                return _operatorDelegate.Value;
            }
            set
            {
                _operatorDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<string> GenericTypeArguments
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> GenericTypeArgumentConstraints
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

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> CodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder> Parameters
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

        public ModelFramework.Objects.Contracts.IClassMethod Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.ClassMethod(Partial, ExtensionMethod, Operator, new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.Build()), CodeStatements.Select(x => x.Build()), Parameters.Select(x => x.Build()), TypeName, IsNullable, IsValueType, ExplicitInterfaceName, ParentTypeFullName);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassMethodBuilder WithPartial(bool partial = true)
        {
            Partial = partial;
            return this;
        }

        public ClassMethodBuilder WithPartial(System.Func<bool> partialDelegate)
        {
            _partialDelegate = new (partialDelegate);
            return this;
        }

        public ClassMethodBuilder WithExtensionMethod(bool extensionMethod = true)
        {
            ExtensionMethod = extensionMethod;
            return this;
        }

        public ClassMethodBuilder WithExtensionMethod(System.Func<bool> extensionMethodDelegate)
        {
            _extensionMethodDelegate = new (extensionMethodDelegate);
            return this;
        }

        public ClassMethodBuilder WithOperator(bool @operator = true)
        {
            Operator = @operator;
            return this;
        }

        public ClassMethodBuilder WithOperator(System.Func<bool> operatorDelegate)
        {
            _operatorDelegate = new (@operatorDelegate);
            return this;
        }

        public ClassMethodBuilder AddGenericTypeArguments(System.Collections.Generic.IEnumerable<string> genericTypeArguments)
        {
            return AddGenericTypeArguments(genericTypeArguments.ToArray());
        }

        public ClassMethodBuilder AddGenericTypeArguments(params string[] genericTypeArguments)
        {
            GenericTypeArguments.AddRange(genericTypeArguments);
            return this;
        }

        public ClassMethodBuilder AddGenericTypeArgumentConstraints(System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints)
        {
            return AddGenericTypeArgumentConstraints(genericTypeArgumentConstraints.ToArray());
        }

        public ClassMethodBuilder AddGenericTypeArgumentConstraints(params string[] genericTypeArgumentConstraints)
        {
            GenericTypeArgumentConstraints.AddRange(genericTypeArgumentConstraints);
            return this;
        }

        public ClassMethodBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassMethodBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassMethodBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassMethodBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassMethodBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassMethodBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassMethodBuilder WithVirtual(System.Func<bool> virtualDelegate)
        {
            _virtualDelegate = new (@virtualDelegate);
            return this;
        }

        public ClassMethodBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassMethodBuilder WithAbstract(System.Func<bool> abstractDelegate)
        {
            _abstractDelegate = new (@abstractDelegate);
            return this;
        }

        public ClassMethodBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassMethodBuilder WithProtected(System.Func<bool> protectedDelegate)
        {
            _protectedDelegate = new (@protectedDelegate);
            return this;
        }

        public ClassMethodBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassMethodBuilder WithOverride(System.Func<bool> overrideDelegate)
        {
            _overrideDelegate = new (@overrideDelegate);
            return this;
        }

        public ClassMethodBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassMethodBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public ClassMethodBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ClassMethodBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ClassMethodBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassMethodBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassMethodBuilder AddCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }

        public ClassMethodBuilder AddCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] codeStatements)
        {
            CodeStatements.AddRange(codeStatements);
            return this;
        }

        public ClassMethodBuilder AddLiteralCodeStatements(params string[] statements)
        {
            AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));
            return this;
        }

        public ClassMethodBuilder AddLiteralCodeStatements(System.Collections.Generic.IEnumerable<string> statements)
        {
            AddLiteralCodeStatements(statements.ToArray());
            return this;
        }

        public ClassMethodBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public ClassMethodBuilder AddParameters(params ModelFramework.Objects.Builders.ParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, System.Type type)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type));
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, string typeName)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName));
            return this;
        }

        public ClassMethodBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ClassMethodBuilder WithTypeName(System.Func<string> typeNameDelegate)
        {
            _typeNameDelegate = new (typeNameDelegate);
            return this;
        }

        public ClassMethodBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName; IsValueType = type.IsValueType || type.IsEnum;
            return this;
        }

        public ClassMethodBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassMethodBuilder WithIsNullable(System.Func<bool> isNullableDelegate)
        {
            _isNullableDelegate = new (isNullableDelegate);
            return this;
        }

        public ClassMethodBuilder WithIsValueType(bool isValueType = true)
        {
            IsValueType = isValueType;
            return this;
        }

        public ClassMethodBuilder WithIsValueType(System.Func<bool> isValueTypeDelegate)
        {
            _isValueTypeDelegate = new (isValueTypeDelegate);
            return this;
        }

        public ClassMethodBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }

        public ClassMethodBuilder WithExplicitInterfaceName(System.Func<string> explicitInterfaceNameDelegate)
        {
            _explicitInterfaceNameDelegate = new (explicitInterfaceNameDelegate);
            return this;
        }

        public ClassMethodBuilder WithParentTypeFullName(string parentTypeFullName)
        {
            ParentTypeFullName = parentTypeFullName;
            return this;
        }

        public ClassMethodBuilder WithParentTypeFullName(System.Func<string> parentTypeFullNameDelegate)
        {
            _parentTypeFullNameDelegate = new (parentTypeFullNameDelegate);
            return this;
        }

        public ClassMethodBuilder()
        {
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _partialDelegate = new (() => default);
            _extensionMethodDelegate = new (() => default);
            _operatorDelegate = new (() => default);
            _staticDelegate = new (() => default);
            _virtualDelegate = new (() => default);
            _abstractDelegate = new (() => default);
            _protectedDelegate = new (() => default);
            _overrideDelegate = new (() => default);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Public);
            _nameDelegate = new (() => string.Empty);
            _typeNameDelegate = new (() => string.Empty);
            _isNullableDelegate = new (() => default);
            _isValueTypeDelegate = new (() => default);
            _explicitInterfaceNameDelegate = new (() => string.Empty);
            _parentTypeFullNameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassMethodBuilder(ModelFramework.Objects.Contracts.IClassMethod source)
        {
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            _partialDelegate = new (() => source.Partial);
            _extensionMethodDelegate = new (() => source.ExtensionMethod);
            _operatorDelegate = new (() => source.Operator);
            GenericTypeArguments.AddRange(source.GenericTypeArguments);
            GenericTypeArgumentConstraints.AddRange(source.GenericTypeArgumentConstraints);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _staticDelegate = new (() => source.Static);
            _virtualDelegate = new (() => source.Virtual);
            _abstractDelegate = new (() => source.Abstract);
            _protectedDelegate = new (() => source.Protected);
            _overrideDelegate = new (() => source.Override);
            _visibilityDelegate = new (() => source.Visibility);
            _nameDelegate = new (() => source.Name);
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.ParameterBuilder(x)));
            _typeNameDelegate = new (() => source.TypeName);
            _isNullableDelegate = new (() => source.IsNullable);
            _isValueTypeDelegate = new (() => source.IsValueType);
            _explicitInterfaceNameDelegate = new (() => source.ExplicitInterfaceName);
            _parentTypeFullNameDelegate = new (() => source.ParentTypeFullName);
        }

        protected System.Lazy<bool> _partialDelegate;

        protected System.Lazy<bool> _extensionMethodDelegate;

        protected System.Lazy<bool> _operatorDelegate;

        protected System.Lazy<bool> _staticDelegate;

        protected System.Lazy<bool> _virtualDelegate;

        protected System.Lazy<bool> _abstractDelegate;

        protected System.Lazy<bool> _protectedDelegate;

        protected System.Lazy<bool> _overrideDelegate;

        protected System.Lazy<ModelFramework.Objects.Contracts.Visibility> _visibilityDelegate;

        protected System.Lazy<string> _nameDelegate;

        protected System.Lazy<string> _typeNameDelegate;

        protected System.Lazy<bool> _isNullableDelegate;

        protected System.Lazy<bool> _isValueTypeDelegate;

        protected System.Lazy<string> _explicitInterfaceNameDelegate;

        protected System.Lazy<string> _parentTypeFullNameDelegate;
    }
#nullable restore
}

