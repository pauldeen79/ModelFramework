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
    public partial class ClassConstructorBuilder
    {
        public string ChainCall
        {
            get
            {
                return _chainCallDelegate.Value;
            }
            set
            {
                _chainCallDelegate = new (() => value);
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

        public ClassConstructorBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassConstructorBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassConstructorBuilder AddCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] codeStatements)
        {
            CodeStatements.AddRange(codeStatements);
            return this;
        }

        public ClassConstructorBuilder AddCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }

        public ClassConstructorBuilder AddLiteralCodeStatements(System.Collections.Generic.IEnumerable<string> statements)
        {
            AddLiteralCodeStatements(statements.ToArray());
            return this;
        }

        public ClassConstructorBuilder AddLiteralCodeStatements(params string[] statements)
        {
            AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));
            return this;
        }

        public ClassConstructorBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassConstructorBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassConstructorBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassConstructorBuilder AddParameter(string name, string typeName)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName));
            return this;
        }

        public ClassConstructorBuilder AddParameter(string name, System.Type type)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type));
            return this;
        }

        public ClassConstructorBuilder AddParameters(params ModelFramework.Objects.Builders.ParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public ClassConstructorBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public ModelFramework.Objects.Contracts.IClassConstructor Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.ClassConstructor(ChainCall, Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Attributes.Select(x => x.Build()), CodeStatements.Select(x => x.Build()), Parameters.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassConstructorBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassConstructorBuilder WithAbstract(System.Func<bool> abstractDelegate)
        {
            _abstractDelegate = new (@abstractDelegate);
            return this;
        }

        public ClassConstructorBuilder WithChainCall(System.Func<string> chainCallDelegate)
        {
            _chainCallDelegate = new (chainCallDelegate);
            return this;
        }

        public ClassConstructorBuilder WithChainCall(string chainCall)
        {
            ChainCall = chainCall;
            return this;
        }

        public ClassConstructorBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassConstructorBuilder WithOverride(System.Func<bool> overrideDelegate)
        {
            _overrideDelegate = new (@overrideDelegate);
            return this;
        }

        public ClassConstructorBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassConstructorBuilder WithProtected(System.Func<bool> protectedDelegate)
        {
            _protectedDelegate = new (@protectedDelegate);
            return this;
        }

        public ClassConstructorBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassConstructorBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassConstructorBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassConstructorBuilder WithVirtual(System.Func<bool> virtualDelegate)
        {
            _virtualDelegate = new (@virtualDelegate);
            return this;
        }

        public ClassConstructorBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassConstructorBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public ClassConstructorBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _chainCallDelegate = new (() => string.Empty);
            _staticDelegate = new (() => default);
            _virtualDelegate = new (() => default);
            _abstractDelegate = new (() => default);
            _protectedDelegate = new (() => default);
            _overrideDelegate = new (() => default);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Public);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassConstructorBuilder(ModelFramework.Objects.Contracts.IClassConstructor source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            _chainCallDelegate = new (() => source.ChainCall);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _staticDelegate = new (() => source.Static);
            _virtualDelegate = new (() => source.Virtual);
            _abstractDelegate = new (() => source.Abstract);
            _protectedDelegate = new (() => source.Protected);
            _overrideDelegate = new (() => source.Override);
            _visibilityDelegate = new (() => source.Visibility);
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.ParameterBuilder(x)));
        }

        protected System.Lazy<string> _chainCallDelegate;

        protected System.Lazy<bool> _staticDelegate;

        protected System.Lazy<bool> _virtualDelegate;

        protected System.Lazy<bool> _abstractDelegate;

        protected System.Lazy<bool> _protectedDelegate;

        protected System.Lazy<bool> _overrideDelegate;

        protected System.Lazy<ModelFramework.Objects.Contracts.Visibility> _visibilityDelegate;
    }
#nullable restore
}

