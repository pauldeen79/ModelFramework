﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.2
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
            get;
            set;
        }

        public bool Virtual
        {
            get;
            set;
        }

        public bool Abstract
        {
            get;
            set;
        }

        public bool Protected
        {
            get;
            set;
        }

        public bool Override
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
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

        public ModelFramework.Objects.Contracts.IClassConstructor Build()
        {
            return new ModelFramework.Objects.ClassConstructor(ChainCall, Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Attributes.Select(x => x.Build()), CodeStatements.Select(x => x.Build()), Parameters.Select(x => x.Build()));
        }

        public ClassConstructorBuilder WithChainCall(string chainCall)
        {
            ChainCall = chainCall;
            return this;
        }

        public ClassConstructorBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassConstructorBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassConstructorBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassConstructorBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassConstructorBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassConstructorBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassConstructorBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassConstructorBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassConstructorBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassConstructorBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassConstructorBuilder AddCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }

        public ClassConstructorBuilder AddCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] codeStatements)
        {
            CodeStatements.AddRange(codeStatements);
            return this;
        }

        public ClassConstructorBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public ClassConstructorBuilder AddParameters(params ModelFramework.Objects.Builders.ParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public ClassConstructorBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassConstructorBuilder AddParameter(string name, System.Type type)
        {
            return AddParameters(new ParameterBuilder().WithName(name).WithType(type));
        }

        public ClassConstructorBuilder AddParameter(string name, string typeName)
        {
            return AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));
        }

        public ClassConstructorBuilder AddLiteralCodeStatements(params string[] statements)
        {
            return AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));
        }

        public ClassConstructorBuilder AddLiteralCodeStatements(System.Collections.Generic.IEnumerable<string> statements)
        {
            return AddLiteralCodeStatements(statements.ToArray());
        }

        public ClassConstructorBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            ChainCall = string.Empty;
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Visibility = ModelFramework.Objects.Contracts.Visibility.Public;
        }

        public ClassConstructorBuilder(ModelFramework.Objects.Contracts.IClassConstructor source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            ChainCall = source.ChainCall;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.ParameterBuilder(x)));
        }
    }
#nullable restore
}

